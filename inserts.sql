INSERT INTO Users (Name, Email, Password, IdStatus, IdUserRole) VALUES
('Admin User', 'admin@example.com', 'AYxyDpzYcdEgrMv9Onfee2JIe9KAfuTV2w3aj/Bdc6lcM5Oesa5ULLYxXYQRA3Gg', 1, 1),
('John Doe', 'john.doe@example.com', 'AYxyDpzYcdEgrMv9Onfee2JIe9KAfuTV2w3aj/Bdc6lcM5Oesa5ULLYxXYQRA3Gg', 1, 2),
('Jane Smith', 'jane.smith@example.com', 'AYxyDpzYcdEgrMv9Onfee2JIe9KAfuTV2w3aj/Bdc6lcM5Oesa5ULLYxXYQRA3Gg', 1, 2),
('Robert Johnson', 'robert.j@example.com', 'AYxyDpzYcdEgrMv9Onfee2JIe9KAfuTV2w3aj/Bdc6lcM5Oesa5ULLYxXYQRA3Gg', 2, 2),
('Emily Davis', 'emily.d@example.com', 'AYxyDpzYcdEgrMv9Onfee2JIe9KAfuTV2w3aj/Bdc6lcM5Oesa5ULLYxXYQRA3Gg', 1, 2);
-- password123

INSERT INTO Categories (Name) VALUES
('Electrónicos'),
('Ropa'),
('Hogar'),
('Deportes'),
('Juguetes');

INSERT INTO Products (Name, Description, Price, Quantity, IdCategory, IdStatus) VALUES
('Smartphone X', 'Último modelo con cámara de 48MP', 799.99, 50, 1, 1),
('Laptop Pro', '16GB RAM, 512GB SSD', 1299.99, 30, 1, 1),
('Camiseta algodón', '100% algodón orgánico, varios colores', 24.99, 100, 2, 1),
('Sofá de 3 plazas', 'Tela resistente, color gris', 599.00, 15, 3, 1),
('Balón de fútbol', 'Tamaño oficial, FIFA aprobado', 29.99, 75, 4, 1),
('Muñeca articulada', 'Incluye 3 vestidos', 39.99, 40, 5, 1),
('Auriculares inalámbricos', 'Cancelación de ruido', 199.99, 60, 1, 1),
('Jeans ajustados', 'Modelo slim fit, color azul', 49.99, 85, 2, 1),
('Juego de sartenes', '6 piezas, antiadherente', 89.99, 25, 3, 1),
('Raqueta de tenis', 'Peso ligero, grip ergonómico', 79.99, 35, 4, 1);