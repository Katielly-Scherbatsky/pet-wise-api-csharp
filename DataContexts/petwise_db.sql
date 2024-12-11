CREATE DATABASE IF NOT EXISTS PetWiseDB;
USE PetWiseDB;

-- Tabela Usuario
CREATE TABLE Usuario (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    senha VARCHAR(255) NOT NULL,
    criado_em DATETIME DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Tabela Animal
CREATE TABLE Animal (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome VARCHAR(255) NOT NULL,
    data_nascimento DATETIME,
    usuario_id INT NOT NULL,
    criado_em DATETIME DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (usuario_id) REFERENCES Usuario(id)
);

-- Tabela Pesagem
CREATE TABLE Pesagem (
    id INT AUTO_INCREMENT PRIMARY KEY,
    peso DECIMAL(10, 2) NOT NULL,
    data_pesagem DATETIME NOT NULL,
    observacoes TEXT,
    criado_em DATETIME DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    animal_id INT NOT NULL,
    FOREIGN KEY (animal_id) REFERENCES Animal(id)
);

-- Tabela Vacinacao
CREATE TABLE Vacinacao (
    id INT AUTO_INCREMENT PRIMARY KEY,
    nome_vacina VARCHAR(255) NOT NULL,
    data_aplicacao DATETIME NOT NULL,
    data_proxima_aplicacao DATETIME,
    observacoes TEXT,
    criado_em DATETIME DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    animal_id INT NOT NULL,
    FOREIGN KEY (animal_id) REFERENCES Animal(id)
);

-- Tabela Tratamento
CREATE TABLE Tratamento (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tipo_tratamento VARCHAR(255) NOT NULL,
    data_aplicacao DATETIME NOT NULL,
    data_proxima_aplicacao DATETIME,
    observacoes TEXT,
    criado_em DATETIME DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    animal_id INT NOT NULL,
    FOREIGN KEY (animal_id) REFERENCES Animal(id)
);

-- Tabela Suplementacao
CREATE TABLE Suplementacao (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tipo_suplementacao VARCHAR(255) NOT NULL,
    numero_doses INT NOT NULL,
    data_aplicacao DATETIME NOT NULL,
    data_proxima_aplicacao DATETIME,
    observacoes TEXT,
    criado_em DATETIME DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    animal_id INT NOT NULL,
    FOREIGN KEY (animal_id) REFERENCES Animal(id)
);

-- Tabela BanhoTosa
CREATE TABLE Banho_tosa (
    id INT AUTO_INCREMENT PRIMARY KEY,
    executor VARCHAR(255),
    data_servico DATETIME NOT NULL,
    observacoes TEXT,
    criado_em DATETIME DEFAULT CURRENT_TIMESTAMP,
    atualizado_em DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    animal_id INT NOT NULL,
    FOREIGN KEY (animal_id) REFERENCES Animal(id)
);
