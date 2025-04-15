-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Hôte : localhost
-- Généré le : mar. 15 avr. 2025 à 06:11
-- Version du serveur : 8.0.31
-- Version de PHP : 7.4.33

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `heavy_app_e5`
--

-- --------------------------------------------------------

--
-- Structure de la table `brand`
--

CREATE TABLE `brand` (
  `brand_id` int NOT NULL,
  `name` varchar(32) COLLATE utf8mb4_general_ci NOT NULL,
  `creation_year` year NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='store every brand that rent excavators';

--
-- Déchargement des données de la table `brand`
--

INSERT INTO `brand` (`brand_id`, `name`, `creation_year`) VALUES
(1, 'Caterpillar Inc', '1925'),
(2, 'Volvo Construction Equipment', '1973');

-- --------------------------------------------------------

--
-- Structure de la table `customer`
--

CREATE TABLE `customer` (
  `customer_id` int NOT NULL,
  `first_name` varchar(32) COLLATE utf8mb4_general_ci NOT NULL,
  `last_name` varchar(32) COLLATE utf8mb4_general_ci NOT NULL,
  `email` varchar(64) COLLATE utf8mb4_general_ci NOT NULL,
  `birth_date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='store customer that can rent excavators';

--
-- Déchargement des données de la table `customer`
--

INSERT INTO `customer` (`customer_id`, `first_name`, `last_name`, `email`, `birth_date`) VALUES
(1, 'barrabes', 'eliot', 'eliot.barrabes@gmail.com', '2005-07-09'),
(6, 'william', 'fourcade', 'william.fourcade2sn@gmail.com', '2004-09-21'),
(7, 'Théo', 'Cabrelli', 'theo.cabrelli@orange.com', '2005-06-17'),
(8, 'Remy', 'Bauchet', 'remy.bauchet@dcscale.fr', '2005-04-26');

-- --------------------------------------------------------

--
-- Structure de la table `excavator`
--

CREATE TABLE `excavator` (
  `excavator_id` int NOT NULL,
  `name` varchar(32) COLLATE utf8mb4_general_ci NOT NULL,
  `description` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `brand_id` int NOT NULL,
  `bucket_liters` int NOT NULL,
  `release_year` year NOT NULL,
  `is_used` tinyint(1) NOT NULL DEFAULT '0',
  `daily_price` int NOT NULL,
  `picture` varchar(512) COLLATE utf8mb4_general_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='store every excavator that can be rented';

--
-- Déchargement des données de la table `excavator`
--

INSERT INTO `excavator` (`excavator_id`, `name`, `description`, `brand_id`, `bucket_liters`, `release_year`, `is_used`, `daily_price`, `picture`) VALUES
(1, '340 UHD', 'Cette pelleteuse de démolition va vous permettre de tout détruire', 1, 48, '2019', 1, 39, 'C:\\\\Users\\\\Eliot\\\\Desktop\\\\WPF next gen\\\\TheAmazingExcavatorRentTool_v2\\\\TheAmazingExcavatorRentTool\\\\ExcavatorImages\\\\demol_excav.jpg'),
(2, '300.9D', 'Cette petite pelleteuse est capable de se faufiler dans les maisons et les recoins les plus inaccessibles', 1, 28, '2014', 0, 12, 'C:\\\\Users\\\\Eliot\\\\Desktop\\\\WPF next gen\\\\TheAmazingExcavatorRentTool_v2\\\\TheAmazingExcavatorRentTool\\\\ExcavatorImages\\\\mini_excav.jpg'),
(72, '350 Tier 4 Stage V', 'Cette pelleteuse est fournie avec un énorme godet', 1, 112, '2017', 0, 87, 'C:\\\\Users\\\\Eliot\\\\Desktop\\\\WPF next gen\\\\TheAmazingExcavatorRentTool_v2\\\\TheAmazingExcavatorRentTool\\\\ExcavatorImages\\\\350Tier4.jpg'),
(73, 'EC950F', 'Effectuez une large palette de tâches plus rapidement avec l\'EC950F robuste de Volvo, qui, grâce à une technologie sophistiquée, permet de booster la productivité et le rendement. Que vous travailliez dans une carrière, l\'excavation de masse ou toute autre application, la grande et puissante pelle hydraulique EC950F accroîtra votre rentabilité.', 2, 7, '2023', 0, 21, 'C:\\\\Users\\\\Eliot\\\\Desktop\\\\WPF next gen\\\\TheAmazingExcavatorRentTool_v2\\\\TheAmazingExcavatorRentTool\\\\ExcavatorImages\\\\volvo-ec950f.png'),
(74, 'EC550E', 'L\'EC550E fournit les mêmes niveaux de durabilité et de performance qu\'une machine de 60 tonnes, qui en font un choix parfait pour les opérations de creusement lourdes, l\'excavation de masse et la préparation des grands chantiers. Pouvant se targuer d\'un excellent confort, de temps de fonctionnement exceptionnels, d\'un système électrohydraulique nouvelle génération, d\'une augmentation de 20% de la productivité et d\'une amélioration de 25% du rendement énergétique, cette machine place la barre très haut pour ', 2, 4, '2022', 0, 63, 'C:\\\\Users\\\\Eliot\\\\Desktop\\\\WPF next gen\\\\TheAmazingExcavatorRentTool_v2\\\\TheAmazingExcavatorRentTool\\\\ExcavatorImages\\\\volvo-ec550e.png'),
(75, 'EC380E Hybrid', 'L\'EC550E fournit les mêmes niveaux de durabilité et de performance qu\'une machine de 60 tonnes, qui en font un choix parfait pour les opérations de creusement lourdes, l\'excavation de masse et la préparation des grands chantiers. Pouvant se targuer d\'un excellent confort, de temps de fonctionnement exceptionnels, d\'un système électrohydraulique nouvelle génération, d\'une augmentation de 20% de la productivité et d\'une amélioration de 25% du rendement énergétique.', 2, 2, '2025', 1, 99, 'C:\\\\Users\\\\Eliot\\\\Desktop\\\\WPF next gen\\\\TheAmazingExcavatorRentTool_v2\\\\TheAmazingExcavatorRentTool\\\\ExcavatorImages\\\\volvo-ec380e-hybrid.png'),
(76, 'EW220E', 'Soyez prêts à travailler avec la EW220E, offrant une grande précision de mouvements, pour vous aider à garder une maîtrise totale. Équipée d\'un puissant moteur Volvo et d\'un système hydraulique parfaitement adapté, cette machine offre à la fois de grandes performances, un excellent contrôle et une faible consommation de carburant. Sa polyvalence facilite le travail, y compris dans les applications les plus exigeantes.', 2, 1, '2021', 1, 12, 'C:\\\\Users\\\\Eliot\\\\Desktop\\\\WPF next gen\\\\TheAmazingExcavatorRentTool_v2\\\\TheAmazingExcavatorRentTool\\\\ExcavatorImages\\\\volvo-ew220e.png');

-- --------------------------------------------------------

--
-- Structure de la table `rental`
--

CREATE TABLE `rental` (
  `rental_id` int NOT NULL,
  `customer_id` int NOT NULL,
  `excavator_id` int NOT NULL,
  `start_date` date NOT NULL,
  `return_date` date DEFAULT NULL,
  `price` int DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Déchargement des données de la table `rental`
--

INSERT INTO `rental` (`rental_id`, `customer_id`, `excavator_id`, `start_date`, `return_date`, `price`) VALUES
(55, 6, 76, '2025-04-03', '2025-04-16', 168),
(56, 8, 75, '2025-04-25', '2025-04-30', 594),
(57, 6, 1, '2025-04-12', '2025-04-15', 156);

-- --------------------------------------------------------

--
-- Structure de la table `_user`
--

CREATE TABLE `_user` (
  `user_id` int NOT NULL,
  `username` varchar(32) COLLATE utf8mb4_general_ci NOT NULL,
  `password` varchar(256) COLLATE utf8mb4_general_ci NOT NULL,
  `is_admin` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='Store every user that can access the app';

--
-- Déchargement des données de la table `_user`
--

INSERT INTO `_user` (`user_id`, `username`, `password`, `is_admin`) VALUES
(1, 'Admin', 'wcIksDzZvHtqhtd/XazkAZF2bEhc1V3EjK+ayHMzXW8=', 1),
(2, 'ELIOTB', 'qiQh7zH5zTpKM/Vsj27W8e8XxqA5rIZKAEEQp02HAY0=', 0);

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `brand`
--
ALTER TABLE `brand`
  ADD PRIMARY KEY (`brand_id`);

--
-- Index pour la table `customer`
--
ALTER TABLE `customer`
  ADD PRIMARY KEY (`customer_id`);

--
-- Index pour la table `excavator`
--
ALTER TABLE `excavator`
  ADD PRIMARY KEY (`excavator_id`),
  ADD KEY `brand_id` (`brand_id`);

--
-- Index pour la table `rental`
--
ALTER TABLE `rental`
  ADD PRIMARY KEY (`rental_id`),
  ADD KEY `customer_id` (`customer_id`),
  ADD KEY `excavator_id` (`excavator_id`);

--
-- Index pour la table `_user`
--
ALTER TABLE `_user`
  ADD PRIMARY KEY (`user_id`);

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `brand`
--
ALTER TABLE `brand`
  MODIFY `brand_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT pour la table `customer`
--
ALTER TABLE `customer`
  MODIFY `customer_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT pour la table `excavator`
--
ALTER TABLE `excavator`
  MODIFY `excavator_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=80;

--
-- AUTO_INCREMENT pour la table `rental`
--
ALTER TABLE `rental`
  MODIFY `rental_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=58;

--
-- AUTO_INCREMENT pour la table `_user`
--
ALTER TABLE `_user`
  MODIFY `user_id` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `excavator`
--
ALTER TABLE `excavator`
  ADD CONSTRAINT `excavator_ibfk_1` FOREIGN KEY (`brand_id`) REFERENCES `brand` (`brand_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Contraintes pour la table `rental`
--
ALTER TABLE `rental`
  ADD CONSTRAINT `rental_ibfk_1` FOREIGN KEY (`customer_id`) REFERENCES `customer` (`customer_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `rental_ibfk_2` FOREIGN KEY (`excavator_id`) REFERENCES `excavator` (`excavator_id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
