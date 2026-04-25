document.addEventListener("DOMContentLoaded", function () {
    const modalElement = document.getElementById("productoModal");
    const modalContent = document.getElementById("productoModalContent");
    const tableContainer = document.getElementById("productoTableContainer");
    const btnNuevoProducto = document.getElementById("btnNuevoProducto");

    if (!modalElement || !modalContent || !tableContainer) return;

    const productoModal = new bootstrap.Modal(modalElement);

    async function abrirModal(url) {
        const response = await fetch(url);
        if (!response.ok) {
            alert("No se pudo cargar el formulario.");
            return;
        }

        modalContent.innerHTML = await response.text();
        productoModal.show();
    }

    btnNuevoProducto?.addEventListener("click", function () {
        abrirModal("/Producto/CrearModal");
    });

    document.addEventListener("click", function (e) {
        const editBtn = e.target.closest(".btn-edit-producto");
        if (editBtn) {
            abrirModal(`/Producto/EditarModal?id=${editBtn.dataset.id}`);
        }
    });

    document.addEventListener("submit", async function (e) {
        if (e.target && e.target.id === "productoForm") {
            e.preventDefault();

            const form = e.target;
            const formData = new FormData(form);
            const idProducto = formData.get("IdProducto");
            const url = idProducto && idProducto !== "0"
                ? "/Producto/EditarAjax"
                : "/Producto/CrearAjax";

            const response = await fetch(url, {
                method: "POST",
                body: formData
            });

            if (!response.ok) {
                const msg = await response.text();
                alert(msg || "No se pudo guardar el producto.");
                return;
            }

            tableContainer.innerHTML = await response.text();
            productoModal.hide();
        }
    });
});