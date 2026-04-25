document.addEventListener("DOMContentLoaded", function () {
    const modalElement = document.getElementById("proveedorModal");
    const modalContent = document.getElementById("proveedorModalContent");
    const tableContainer = document.getElementById("proveedorTableContainer");
    const btnNuevoProveedor = document.getElementById("btnNuevoProveedor");

    if (!modalElement || !modalContent || !tableContainer) return;

    const proveedorModal = new bootstrap.Modal(modalElement);

    async function abrirModal(url) {
        const response = await fetch(url);

        if (!response.ok) {
            alert("No se pudo cargar el formulario.");
            return;
        }

        const html = await response.text();
        modalContent.innerHTML = html;
        proveedorModal.show();
    }

    btnNuevoProveedor?.addEventListener("click", function () {
        abrirModal("/Proveedor/CrearModal");
    });

    document.addEventListener("click", function (e) {
        const editBtn = e.target.closest(".btn-edit-proveedor");
        if (editBtn) {
            abrirModal(`/Proveedor/EditarModal?id=${editBtn.dataset.id}`);
        }
    });

    document.addEventListener("submit", async function (e) {
        if (e.target && e.target.id === "proveedorForm") {
            e.preventDefault();

            const form = e.target;
            const formData = new FormData(form);
            const idProveedor = formData.get("IdProveedor");

            const url = idProveedor && idProveedor !== "0"
                ? "/Proveedor/EditarAjax"
                : "/Proveedor/CrearAjax";

            const response = await fetch(url, {
                method: "POST",
                body: formData
            });

            if (!response.ok) {
                const msg = await response.text();
                alert(msg || "No se pudo guardar el proveedor.");
                return;
            }

            tableContainer.innerHTML = await response.text();
            proveedorModal.hide();
        }
    });
});