# OrderManagement
Prueba_Completa
Instrucciones para la ejecución del proyecto:

-Descargar la carpeta ZIP y extraer todo
-Abrir Visual Studio, abrir proyecto en la carpeta ya extraída
-Ejecutar Proyecto y direccionarte según al puerto de local host (normalmente https://localhost:7189/) agregar "/swagger" al final
-Endpoints disponibles:
GET/api/Clients
POST/api/Clients
GET/api/Clients/{id}
PUT/api/Clients/{id}
POST/api/Ordenes
GET/api/Products
POST/api/Products
GET/api/Products/{id}
PUT/api/Products/{id}
-Decisiones importantes:
Al momento de crear las respuestas todas llevan un formato escalable (ApiResponse<T>) con propiedades Success, Error, Data etc. para un uso a futuro y sea mejor entendible.
Crear relaciones de uno a muchos, muchos a muchos o uno a uno facilitó el flujo de datos y genera un mejor entendiendo del código.
Habilitar CORS que son políticas para que se pueda editar en el Swagger, adecuado cuando el proyecto está en desarrollo.

-**Script SQL** de creación de BD:
CREATE DATABASE OrderManagementDB;
GO
USE OrderManagementDB;
GO
CREATE TABLE Cliente (
    ClienteId BIGINT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Identidad NVARCHAR(50) NOT NULL UNIQUE,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
CREATE TABLE Producto (
    ProductoId BIGINT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(500),
    Precio DECIMAL(10,2) NOT NULL,
    Existencia INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
);
CREATE TABLE Orden (
    OrdenId BIGINT IDENTITY(1,1) PRIMARY KEY,
    ClienteId BIGINT NOT NULL,
    Impuesto DECIMAL(10,2) NOT NULL DEFAULT 0,
    Subtotal DECIMAL(10,2) NOT NULL DEFAULT 0,
    Total DECIMAL(10,2) NOT NULL DEFAULT 0,
    FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Orden_Cliente FOREIGN KEY (ClienteId) REFERENCES 
Cliente(ClienteId)
);
CREATE TABLE DetalleOrden (
    DetalleOrdenId BIGINT IDENTITY(1,1) PRIMARY KEY,
    OrdenId BIGINT NOT NULL,
    ProductoId BIGINT NOT NULL,
    Cantidad INT NOT NULL,
    Impuesto DECIMAL(10,2) NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL,
    Total DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_DetalleOrden_Orden FOREIGN KEY (OrdenId) REFERENCES 
Orden(OrdenId),
    CONSTRAINT FK_DetalleOrden_Producto FOREIGN KEY (ProductoId) REFERENCES 
Producto(ProductoId)
);-- Índices para performance
CREATE INDEX IX_Orden_ClienteId ON Orden(ClienteId);
CREATE INDEX IX_DetalleOrden_OrdenId ON DetalleOrden(OrdenId);
CREATE INDEX IX_DetalleOrden_ProductoId ON DetalleOrden(ProductoId);-- Datos de prueba
INSERT INTO Cliente (Nombre, Identidad) VALUES
('Juan Pérez', '0801-1990-12345'),
('María González', '0801-1985-67890'),
('Carlos Rodríguez', '0801-1992-11111');
INSERT INTO Producto (Nombre, Descripcion, Precio, Existencia) VALUES
('Laptop Dell XPS 15', 'Laptop de alto rendimiento', 1299.99, 50),
('Mouse Logitech MX Master 3', 'Mouse ergonómico inalámbrico', 99.99, 150),
('Teclado Mecánico Keychron K2', 'Teclado mecánico retroiluminado', 89.99, 75),
('Monitor LG 27" 4K', 'Monitor 4K UHD', 449.99, 30),
('Webcam Logitech C920', 'Webcam Full HD 1080p', 79.99, 100);
