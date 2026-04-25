document.addEventListener("DOMContentLoaded", function () {
    const modalElement = document.getElementById("categoriaModal");
    const modalContent = document.getElementById("categoriaModalContent");
    const listContainer = document.getElementById("categoryListContainer");

    if (!modalElement || !modalContent || !listContainer) return;

    const categoriaModal = new bootstrap.Modal(modalElement);

    async function cargarEditar(id) {
        const response = await fetch(`/Categoria/EditarModal?id=${id}`);
        if (!response.ok) {
            alert("No se pudo cargar la categoría.");
            return;
        }

        const html = await response.text();
        modalContent.innerHTML = html;
        categoriaModal.show();
    }

    async function eliminarCategoria(id) {
        const confirmed = confirm("¿Desea eliminar esta categoría?");
        if (!confirmed) return;

        const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
        const token = tokenInput ? tokenInput.value : "";

        const formData = new FormData();
        formData.append("id", id);
        formData.append("__RequestVerificationToken", token);

        const response = await fetch("/Categoria/EliminarAjax", {
            method: "POST",
            body: formData
        });

        if (!response.ok) {
            const errorText = await response.text();
            alert(errorText || "No se pudo eliminar la categoría.");
            return;
        }

        const html = await response.text();
        listContainer.innerHTML = html;
    }

    document.addEventListener("click", function (e) {
        const editBtn = e.target.closest(".btn-edit-categoria");
        if (editBtn) {
            cargarEditar(editBtn.dataset.id);
            return;
        }

        const deleteBtn = e.target.closest(".btn-delete-categoria");
        if (deleteBtn) {
            eliminarCategoria(deleteBtn.dataset.id);
        }
    });

    document.addEventListener("submit", async function (e) {
        if (e.target && e.target.id === "editarCategoriaForm") {
            e.preventDefault();

            const form = e.target;
            const formData = new FormData(form);

            const response = await fetch("/Categoria/EditarAjax", {
                method: "POST",
                body: formData
            });

            if (!response.ok) {
                const errorText = await response.text();
                alert(errorText || "No se pudo actualizar la categoría.");
                return;
            }

            const html = await response.text();
            listContainer.innerHTML = html;
            categoriaModal.hide();
        }
    });
});