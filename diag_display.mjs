async function main() {
  const created = await fetch(`http://localhost:9222/json/new?${encodeURIComponent("http://localhost:5289/Gastos")}`, { method: "PUT" });
  const target = await created.json();
  const ws = new WebSocket(target.webSocketDebuggerUrl);
  let id = 0;
  const pending = new Map();
  function send(method, params = {}) {
    const thisId = ++id;
    return new Promise((resolve) => { pending.set(thisId, resolve); ws.send(JSON.stringify({ id: thisId, method, params })); });
  }
  await new Promise((resolve) => { ws.onopen = resolve; });
  ws.onmessage = (ev) => { const msg = JSON.parse(ev.data); if (msg.id && pending.has(msg.id)) { pending.get(msg.id)(msg.result); pending.delete(msg.id); } };
  await send("Page.enable");
  await send("Runtime.enable");
  await new Promise(r => setTimeout(r, 1200));
  const r = await send("Runtime.evaluate", {
    expression: `
      (function(){
        const d = document.getElementById('modal-nuevo-gasto');
        const cs = getComputedStyle(d);
        return JSON.stringify({ open: d.open, display: cs.display, position: cs.position, visible: d.getBoundingClientRect() });
      })()
    `,
    returnByValue: true,
  });
  console.log("SIN hacer click, recien cargada la pagina:", r.result.value);
  await send("Page.close");
  ws.close();
  process.exit(0);
}
main().catch(e => { console.error(e); process.exit(1); });
