function actualizarTotales() {
    let totalGeneral = 0;

    /* Paradigma funcional:
       Se recorre una colección de filas para transformar cantidades en subtotales. */
    document.querySelectorAll("tbody tr").forEach(row => {
        const input = row.querySelector(".cantidad-input");
        const subtotalText = row.querySelector(".subtotal-text");

        if (!input || !subtotalText) return;

        const precio = parseFloat(input.dataset.precio) || 0;
        const cantidad = parseInt(input.value) || 0;

        const subtotal = precio * cantidad;

        subtotalText.innerText =
            "₡ " + subtotal.toLocaleString("es-CR", {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            });

        totalGeneral += subtotal;
    });

    const totalElement = document.getElementById("totalGeneral");
    if (totalElement) {
        totalElement.innerText =
            totalGeneral.toLocaleString("es-CR", {
                minimumFractionDigits: 2,
                maximumFractionDigits: 2
            });
    }
}

/* Paradigma dirigido por eventos:
   Este bloque responde a cambios del usuario en los inputs de cantidad. */
document.addEventListener("input", function (e) {
    if (e.target.classList.contains("cantidad-input")) {
        actualizarTotales();
    }
});

// Inicializar al cargar
document.addEventListener("DOMContentLoaded", function () {
    actualizarTotales();
});