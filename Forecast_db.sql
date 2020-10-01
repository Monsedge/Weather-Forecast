--
-- Скрипт сгенерирован Devart dbForge Studio 2020 for MySQL, Версия 9.0.391.0
-- Домашняя страница продукта: http://www.devart.com/ru/dbforge/mysql/studio
-- Дата скрипта: 25.09.2020 16:54:33
-- Версия сервера: 8.0.21
-- Версия клиента: 4.1
--

SET NAMES 'utf8';

CREATE DATABASE forecast
CHARACTER SET utf8mb4
COLLATE utf8mb4_0900_ai_ci;

USE forecast;

DROP TABLE IF EXISTS weathers;

USE forecast;

CREATE TABLE weathers (
  id int NOT NULL AUTO_INCREMENT,
  dates varchar(50) NOT NULL DEFAULT 'Дата',
  names varchar(50) NOT NULL DEFAULT 'В Городе',
  minTemperatures varchar(10) NOT NULL DEFAULT '?',
  maxTemperatures varchar(10) NOT NULL DEFAULT '?',
  descriptions varchar(50) NOT NULL DEFAULT 'Описание',
  PRIMARY KEY (id)
)
ENGINE = INNODB,
AUTO_INCREMENT = 541,
AVG_ROW_LENGTH = 819,
CHARACTER SET utf8mb4,
COLLATE utf8mb4_0900_ai_ci;