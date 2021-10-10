// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//button de ventana transferenciaDestino

inputMonto = document.getElementById("FormMonto");
btnContinuar = document.getElementById("btnContinuar");

inputMonto.addEventListener("input", () => {
    if (inputMonto.value > 0) {
        btnContinuar.disabled = false;
    } else {
        btnContinuar.disabled = true;
    }
});
