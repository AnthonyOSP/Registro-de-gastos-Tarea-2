// Abre/cierra los <dialog> del sitio (por ahora, el modal de "nuevo gasto").
// Cualquier enlace/botón con data-modal-target="id-del-dialog" lo abre; si esa
// página no tiene ningún <dialog> con ese id (todas menos "Mis gastos"), no
// pasa nada y el enlace navega normalmente — así funciona igual sin JavaScript.
document.addEventListener("DOMContentLoaded", function () {
  document.querySelectorAll("[data-modal-target]").forEach(function (trigger) {
    var modal = document.getElementById(trigger.getAttribute("data-modal-target"));
    if (!modal) {
      return;
    }

    trigger.addEventListener("click", function (evento) {
      evento.preventDefault();
      modal.showModal();
    });
  });

  document.querySelectorAll("dialog").forEach(function (modal) {
    // Cerrar con el botón "×" o "Cancelar" dentro del modal.
    modal.querySelectorAll("[data-modal-close]").forEach(function (boton) {
      boton.addEventListener("click", function () {
        modal.close();
      });
    });

    // Cerrar al hacer clic fuera del cuadro (sobre el fondo difuminado).
    modal.addEventListener("click", function (evento) {
      var cuadro = modal.getBoundingClientRect();
      var dentroDelCuadro =
        evento.clientX >= cuadro.left &&
        evento.clientX <= cuadro.right &&
        evento.clientY >= cuadro.top &&
        evento.clientY <= cuadro.bottom;
      if (!dentroDelCuadro) {
        modal.close();
      }
    });
  });
});

// Si el usuario abre el modal, navega a otra página y vuelve con el botón
// "atrás" del navegador, el navegador puede restaurar la página completa
// desde su caché (bfcache) tal como quedó — con el modal todavía marcado
// como abierto. "pageshow" con persisted=true detecta justo ese caso (no
// pasa en una carga normal) y lo cierra.
window.addEventListener("pageshow", function (evento) {
  if (evento.persisted) {
    document.querySelectorAll("dialog[open]").forEach(function (modal) {
      modal.close();
    });
  }
});
