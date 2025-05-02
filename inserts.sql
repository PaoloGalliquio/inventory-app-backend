INSERT INTO Users (Name, Email, Password, IdStatus, IdUserRole) VALUES
('Admin User', 'admin@example.com', 'AYxyDpzYcdEgrMv9Onfee2JIe9KAfuTV2w3aj/Bdc6lcM5Oesa5ULLYxXYQRA3Gg', 1, 1),
('John Doe', 'john.doe@example.com', 'AYxyDpzYcdEgrMv9Onfee2JIe9KAfuTV2w3aj/Bdc6lcM5Oesa5ULLYxXYQRA3Gg', 1, 2),
('Jane Smith', 'jane.smith@example.com', 'AYxyDpzYcdEgrMv9Onfee2JIe9KAfuTV2w3aj/Bdc6lcM5Oesa5ULLYxXYQRA3Gg', 1, 2),
('Robert Johnson', 'robert.j@example.com', 'AYxyDpzYcdEgrMv9Onfee2JIe9KAfuTV2w3aj/Bdc6lcM5Oesa5ULLYxXYQRA3Gg', 2, 2),
('Emily Davis', 'emily.d@example.com', 'AYxyDpzYcdEgrMv9Onfee2JIe9KAfuTV2w3aj/Bdc6lcM5Oesa5ULLYxXYQRA3Gg', 1, 2);
-- password123

INSERT INTO Categories (Name) VALUES
('Electr�nicos'),
('Ropa'),
('Hogar'),
('Deportes'),
('Juguetes');

INSERT INTO Products (Name, Description, Price, Quantity, IdCategory, IdStatus) VALUES
('Smartphone X', '�ltimo modelo con c�mara de 48MP', 799.99, 50, 1, 1),
('Laptop Pro', '16GB RAM, 512GB SSD', 1299.99, 30, 1, 1),
('Camiseta algod�n', '100% algod�n org�nico, varios colores', 24.99, 100, 2, 1),
('Sof� de 3 plazas', 'Tela resistente, color gris', 599.00, 15, 3, 1),
('Bal�n de f�tbol', 'Tama�o oficial, FIFA aprobado', 29.99, 75, 4, 1),
('Mu�eca articulada', 'Incluye 3 vestidos', 39.99, 40, 5, 1),
('Auriculares inal�mbricos', 'Cancelaci�n de ruido', 199.99, 60, 1, 1),
('Jeans ajustados', 'Modelo slim fit, color azul', 49.99, 85, 2, 1),
('Juego de sartenes', '6 piezas, antiadherente', 89.99, 25, 3, 1),
('Raqueta de tenis', 'Peso ligero, grip ergon�mico', 79.99, 35, 4, 1);