document.addEventListener("DOMContentLoaded", function () {
    const modalElement = document.getElementById("clienteModal");
    const modalContent = document.getElementById("clienteModalContent");
    const tableContainer = document.getElementById("clienteTableContainer");
    const btnNuevoCliente = document.getElementById("btnNuevoCliente");

    if (!modalElement || !modalContent || !tableContainer) return;

    const clienteModal = new bootstrap.Modal(modalElement);

    async function abrirModal(url) {
        const response = await fetch(url);

        if (!response.ok) {
            alert("No se pudo cargar el formulario.");
            return;
        }

        const html = await response.text();
        modalContent.innerHTML = html;
        clienteModal.show();
    }

    btnNuevoCliente?.addEventListener("click", function () {
        abrirModal("/Cliente/CrearModal");
    });

    document.addEventListener("click", function (e) {
        const editBtn = e.target.closest(".btn-edit-cliente");
        if (editBtn) {
            abrirModal(`/Cliente/EditarModal?id=${editBtn.dataset.id}`);
        }
    });

    document.addEventListener("submit", async function (e) {
        if (e.target && e.target.id === "clienteForm") {
            e.preventDefault();

            const form = e.target;
            const formData = new FormData(form);
            const idUsuario = formData.get("IdUsuario");
            const url = idUsuario && idUsuario !== "0"
                ? "/Cliente/EditarAjax"
                : "/Cliente/CrearAjax";

            const response = await fetch(url, {
                method: "POST",
                body: formData
            });

            if (!response.ok) {
                const msg = await response.text();
                alert(msg || "No se pudo guardar el cliente.");
                return;
            }

            tableContainer.innerHTML = await response.text();
            clienteModal.hide();
        }
    });
});