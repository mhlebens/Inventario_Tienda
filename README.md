# 🌸 CherryBloom

Aplicación web para la gestión y venta de productos cosméticos, desarrollada con ASP.NET, Dapper y SQL Server.

---

## 📖 Descripción

**CherryBloom** es una plataforma web que permite administrar una tienda de productos cosméticos, integrando funcionalidades clave tanto para la gestión interna como para la interacción con clientes.

## 🧪 Funcionalidades principales

* Registro y gestion de clientes y proveedores
* Gestión de productos e inventario (stock, edición y eliminación)
* Administración de categorías
* Registro de ventas y su historial
* Visualización detallada de productos

---

## 🏗️ Arquitectura del proyecto

El proyecto sigue el patrón **MVC (Modelo - Vista - Controlador)**:

* **Models**: Representan la estructura de los datos (Producto, Cliente, Proveedor, etc.)
* **Views**: Interfaces de usuario construidas con Razor (.cshtml)
* **Controllers**: Manejan la lógica de negocio y la comunicación entre modelos y vistas
* **Data**: Configuración de acceso a datos
* **Service**: capa de servicios
* **ViewModels**: Objetos para transportar datos entre vistas y controladores

---

## ⚙️ Tecnologías utilizadas

* Lenguaje: C#
* Framework: ASP.NET MVC
* Acceso a datos: Dapper
* Base de datos: SQL Server Management studio
* Entorno de desarrollo: Visual Studio

---

## 📦 Requisitos

Antes de ejecutar el proyecto, asegúrese de tener instalado:

* Visual Studio 2026 esto por posibles incompatibilidades.
* .NET SDK 10.0
* SQL Server (motor de base de datos) 
* SQL Server Management Studio (SSMS)
  
---
## 🗄️ Configuración de la base de datos

1. Copiar el codigo de la base de datos situada en Inventario_Tienda/Database/SQLTiendaDB.sql
2. Abrir SQL server y seleccionar crear un query nuevo.
3. Pegar el codigo en el query, y se ejecuta el scripts SQL para la creacion de tablas, relaciones, procedimientos, etc.

---

## 🚀 Instalación

1. Clonar el repositorio:

```
git clone https://github.com/usuario/CherryBloom.git
```

2. Abrir el proyecto en Visual Studio

3. Configurar la cadena de conexión en:

```
appsettings.json
```

Ejemplo:

```
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=TiendaDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

---

## ▶️ Ejecución

Desde Visual Studio:

* **Start** es la flecha verde superior.
* Su navegador va a abrir una pestaña donde podra visualizar el proyecto.

---

## 📂 Estructura del proyecto

```bash
Inventario_Tienda/
│── Controllers/
│   ├── CategoriaController.cs
│   ├── ClienteController.cs
│   ├── HomeController.cs
│   ├── ProductoController.cs
│   ├── ProveedorController.cs
│   ├── VentaController.cs
│
│── Models/
│   ├── Categoria.cs
│   ├── Compra.cs
│   ├── DetalleCompra.cs
│   ├── ErrorViewModel.cs
│   ├── Producto.cs
│   ├── Proveedor.cs
│   ├── Usuario.cs
│
│── Views/
│   ├── Categoria/
|   │   ├── Crear.cshtml
|   │   ├── Editar.cshtml
|   │   ├── Index.cshtml
│   ├── Cliente/
|   │   ├── Crear.cshtml
|   │   ├── Editar.cshtml
|   │   ├── Index.cshtml
│   ├── Home/
|   │   ├── Index.cshtml
|   │   ├── Provacy.cshtml
│   ├── Producto/
|   │   ├── Crear.cshtml
|   │   ├── Editar.cshtml
|   │   ├── VerProducto.cshtml
│   ├── Proveedor/
|   │   ├── Crear.cshtml
|   │   ├── Editar.cshtml
|   │   ├── Index.cshtml
│   ├── Venta/
|   │   ├── Historial.cshtml
|   │   ├── Registrar.cshtml
│   ├── Shared/
|   │   ├── Layout.cshtml
|   │   ├── _ValidationScriptsPartial.cshtml
|   │   ├── _ViewStart.cshtml
|   │   ├── Error.cshtml
│
│── Data/
│── Service/
│   ├── VentaServive.cs
│── ViewModels/
│   ├── DetalleVentaViewModel.cs
│   ├── VentaHistorialViewModel.cs
│   ├── VentaViewModel.cs
│── wwwroot/
│── appsettings.json
│── Program.cs
```

---

## 👩‍💻 Autor

Desarrollado por: Monica Durán

---
