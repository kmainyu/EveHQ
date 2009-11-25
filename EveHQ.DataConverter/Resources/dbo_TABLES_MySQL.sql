-- MySQL dump 10.10
--
-- Host: localhost    Database: mssql
-- ------------------------------------------------------
-- Server version	5.0.24a

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `agtAgentTypes`
--

DROP TABLE IF EXISTS `agtAgentTypes`;
CREATE TABLE `agtAgentTypes` (
  `agentTypeID` tinyint(3) unsigned NOT NULL,
  `agentType` varchar(50) default NULL,
  PRIMARY KEY  (`agentTypeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `agtAgents`
--

DROP TABLE IF EXISTS `agtAgents`;
CREATE TABLE `agtAgents` (
  `agentID` int(11) NOT NULL,
  `divisionID` int(11) default NULL,
  `corporationID` int(11) default NULL,
  `stationID` int(11) default NULL,
  `level` tinyint(4) default NULL,
  `quality` smallint(6) default NULL,
  `agentTypeID` tinyint(3) unsigned default NULL,
  PRIMARY KEY  (`agentID`),
  KEY `agents_IX_corporation` (`corporationID`),
  KEY `agents_IX_station` (`stationID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `agtConfig`
--

DROP TABLE IF EXISTS `agtConfig`;
CREATE TABLE `agtConfig` (
  `agentID` int(11) NOT NULL,
  `k` varchar(50) NOT NULL,
  `v` varchar(4000) default NULL,
  PRIMARY KEY  (`agentID`,`k`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `agtResearchAgents`
--

DROP TABLE IF EXISTS `agtResearchAgents`;
CREATE TABLE `agtResearchAgents` (
  `agentID` int(11) NOT NULL,
  `typeID` smallint(6) NOT NULL,
  PRIMARY KEY  (`agentID`,`typeID`),
  KEY `agtResearchAgents_IX_type` (`typeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `chrAncestries`
--

DROP TABLE IF EXISTS `chrAncestries`;
CREATE TABLE `chrAncestries` (
  `ancestryID` tinyint(3) unsigned NOT NULL,
  `ancestryName` varchar(100) character set utf8 default NULL,
  `bloodlineID` tinyint(3) unsigned default NULL,
  `description` varchar(1000) character set utf8 default NULL,
  `perception` tinyint(4) default NULL,
  `willpower` tinyint(4) default NULL,
  `charisma` tinyint(4) default NULL,
  `memory` tinyint(4) default NULL,
  `intelligence` tinyint(4) default NULL,
  `graphicID` smallint(6) default NULL,
  `shortDescription` varchar(500) character set utf8 default NULL,
  PRIMARY KEY  (`ancestryID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `chrAttributes`
--

DROP TABLE IF EXISTS `chrAttributes`;
CREATE TABLE `chrAttributes` (
  `attributeID` tinyint(3) unsigned NOT NULL,
  `attributeName` varchar(100) default NULL,
  `description` varchar(1000) default NULL,
  `graphicID` smallint(6) default NULL,
  `shortDescription` varchar(500) character set utf8 default NULL,
  `notes` varchar(500) character set utf8 default NULL,
  PRIMARY KEY  (`attributeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `chrBloodlines`
--

DROP TABLE IF EXISTS `chrBloodlines`;
CREATE TABLE `chrBloodlines` (
  `bloodlineID` tinyint(3) unsigned NOT NULL,
  `bloodlineName` varchar(100) character set utf8 default NULL,
  `raceID` tinyint(3) unsigned default NULL,
  `description` varchar(1000) character set utf8 default NULL,
  `maleDescription` varchar(1000) character set utf8 default NULL,
  `femaleDescription` varchar(1000) character set utf8 default NULL,
  `shipTypeID` smallint(6) default NULL,
  `corporationID` int(11) default NULL,
  `perception` tinyint(4) default NULL,
  `willpower` tinyint(4) default NULL,
  `charisma` tinyint(4) default NULL,
  `memory` tinyint(4) default NULL,
  `intelligence` tinyint(4) default NULL,
  `graphicID` smallint(6) default NULL,
  `shortDescription` varchar(500) character set utf8 default NULL,
  `shortMaleDescription` varchar(500) character set utf8 default NULL,
  `shortFemaleDescription` varchar(500) character set utf8 default NULL,
  PRIMARY KEY  (`bloodlineID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `chrCareerSkills`
--

DROP TABLE IF EXISTS `chrCareerSkills`;
CREATE TABLE `chrCareerSkills` (
  `careerID` tinyint(3) unsigned NOT NULL,
  `skillTypeID` smallint(6) NOT NULL,
  `levels` tinyint(4) default NULL,
  PRIMARY KEY  (`careerID`,`skillTypeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `chrCareerSpecialities`
--

DROP TABLE IF EXISTS `chrCareerSpecialities`;
CREATE TABLE `chrCareerSpecialities` (
  `specialityID` tinyint(3) unsigned NOT NULL,
  `careerID` tinyint(3) unsigned default NULL,
  `specialityName` varchar(100) default NULL,
  `description` varchar(2000) default NULL,
  `shortDescription` varchar(500) default NULL,
  `graphicID` int(11) default NULL,
  `departmentID` int(11) default NULL,
  PRIMARY KEY  (`specialityID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `chrCareerSpecialitySkills`
--

DROP TABLE IF EXISTS `chrCareerSpecialitySkills`;
CREATE TABLE `chrCareerSpecialitySkills` (
  `specialityID` tinyint(3) unsigned NOT NULL,
  `skillTypeID` smallint(6) NOT NULL,
  `levels` tinyint(4) default NULL,
  PRIMARY KEY  (`specialityID`,`skillTypeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `chrCareers`
--

DROP TABLE IF EXISTS `chrCareers`;
CREATE TABLE `chrCareers` (
  `raceID` tinyint(3) unsigned default NULL,
  `careerID` tinyint(3) unsigned NOT NULL,
  `careerName` varchar(100) character set utf8 default NULL,
  `description` varchar(2000) character set utf8 default NULL,
  `shortDescription` varchar(500) character set utf8 default NULL,
  `graphicID` smallint(6) default NULL,
  `schoolID` tinyint(3) unsigned default NULL,
  PRIMARY KEY  (`careerID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `chrFactions`
--

DROP TABLE IF EXISTS `chrFactions`;
CREATE TABLE `chrFactions` (
  `factionID` int(11) NOT NULL,
  `factionName` varchar(100) default NULL,
  `description` varchar(1000) default NULL,
  `raceIDs` int(11) default NULL,
  `solarSystemID` int(11) default NULL,
  `corporationID` int(11) default NULL,
  `sizeFactor` float default NULL,
  `stationCount` smallint(6) default NULL,
  `stationSystemCount` smallint(6) default NULL,
  `militiaCorporationID` int(11) default NULL,
  PRIMARY KEY  (`factionID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `chrRaceSkills`
--

DROP TABLE IF EXISTS `chrRaceSkills`;
CREATE TABLE `chrRaceSkills` (
  `raceID` tinyint(3) unsigned NOT NULL,
  `skillTypeID` smallint(6) NOT NULL,
  `levels` tinyint(4) default NULL,
  PRIMARY KEY  (`raceID`,`skillTypeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `chrRaces`
--

DROP TABLE IF EXISTS `chrRaces`;
CREATE TABLE `chrRaces` (
  `raceID` tinyint(3) unsigned NOT NULL,
  `raceName` varchar(100) default NULL,
  `description` varchar(1000) default NULL,
  `graphicID` int(11) default NULL,
  `shortDescription` varchar(500) default NULL,
  PRIMARY KEY  (`raceID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `chrSchoolAgents`
--

DROP TABLE IF EXISTS `chrSchoolAgents`;
CREATE TABLE `chrSchoolAgents` (
  `schoolID` tinyint(3) unsigned NOT NULL,
  `agentIndex` tinyint(4) NOT NULL,
  `agentID` int(11) default NULL,
  PRIMARY KEY  (`schoolID`,`agentIndex`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `chrSchools`
--

DROP TABLE IF EXISTS `chrSchools`;
CREATE TABLE `chrSchools` (
  `raceID` tinyint(3) unsigned default NULL,
  `schoolID` tinyint(3) unsigned NOT NULL,
  `schoolName` varchar(100) character set utf8 default NULL,
  `description` varchar(1000) character set utf8 default NULL,
  `graphicID` int(11) default NULL,
  `corporationID` int(11) default NULL,
  `agentID` int(11) default NULL,
  `newAgentID` int(11) default NULL,
  `careerID` tinyint(3) unsigned default NULL,
  PRIMARY KEY  (`schoolID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `crpActivities`
--

DROP TABLE IF EXISTS `crpActivities`;
CREATE TABLE `crpActivities` (
  `activityID` tinyint(3) unsigned NOT NULL,
  `activityName` varchar(100) character set utf8 default NULL,
  `description` varchar(1000) character set utf8 default NULL,
  PRIMARY KEY  (`activityID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `crpNPCCorporationDivisions`
--

DROP TABLE IF EXISTS `crpNPCCorporationDivisions`;
CREATE TABLE `crpNPCCorporationDivisions` (
  `corporationID` int(11) NOT NULL,
  `divisionID` tinyint(3) unsigned NOT NULL,
  `divisionNumber` tinyint(4) default NULL,
  `size` tinyint(4) default NULL,
  `leaderID` int(11) default NULL,
  PRIMARY KEY  (`corporationID`,`divisionID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `crpNPCCorporationResearchFields`
--

DROP TABLE IF EXISTS `crpNPCCorporationResearchFields`;
CREATE TABLE `crpNPCCorporationResearchFields` (
  `skillID` smallint(6) NOT NULL,
  `corporationID` int(11) NOT NULL,
  `supplierTypes` tinyint(4) default NULL,
  PRIMARY KEY  (`skillID`,`corporationID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `crpNPCCorporations`
--

DROP TABLE IF EXISTS `crpNPCCorporations`;
CREATE TABLE `crpNPCCorporations` (
  `corporationID` int(11) NOT NULL,
  `mainActivityID` tinyint(3) unsigned default NULL,
  `secondaryActivityID` tinyint(3) unsigned default NULL,
  `size` char(1) default NULL,
  `extent` char(1) default NULL,
  `solarSystemID` int(11) default NULL,
  `investorID1` int(11) default NULL,
  `investorShares1` tinyint(4) default NULL,
  `investorID2` int(11) default NULL,
  `investorShares2` tinyint(4) default NULL,
  `investorID3` int(11) default NULL,
  `investorShares3` tinyint(4) default NULL,
  `investorID4` int(11) default NULL,
  `investorShares4` tinyint(4) default NULL,
  `friendID` int(11) default NULL,
  `enemyID` int(11) default NULL,
  `publicShares` bigint(20) default NULL,
  `initialPrice` int(11) default NULL,
  `minSecurity` float default NULL,
  `scattered` tinyint(1) default NULL,
  `fringe` tinyint(4) default NULL,
  `corridor` tinyint(4) default NULL,
  `hub` tinyint(4) default NULL,
  `border` tinyint(4) default NULL,
  `factionID` int(11) default NULL,
  `sizeFactor` float default NULL,
  `stationCount` smallint(6) default NULL,
  `stationSystemCount` smallint(6) default NULL,
  PRIMARY KEY  (`corporationID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `crpNPCDivisions`
--

DROP TABLE IF EXISTS `crpNPCDivisions`;
CREATE TABLE `crpNPCDivisions` (
  `divisionID` tinyint(3) unsigned NOT NULL,
  `divisionName` varchar(100) character set utf8 default NULL,
  `description` varchar(1000) character set utf8 default NULL,
  `leaderType` varchar(100) character set utf8 default NULL,
  PRIMARY KEY  (`divisionID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `dgmAttributeCategories`
--

DROP TABLE IF EXISTS `dgmAttributeCategories`;
CREATE TABLE `dgmAttributeCategories` (
  `categoryID` tinyint(3) unsigned NOT NULL,
  `categoryName` varchar(50) character set utf8 default NULL,
  `categoryDescription` varchar(200) character set utf8 default NULL,
  PRIMARY KEY  (`categoryID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `dgmAttributeTypes`
--

DROP TABLE IF EXISTS `dgmAttributeTypes`;
CREATE TABLE `dgmAttributeTypes` (
  `attributeID` smallint(6) NOT NULL,
  `attributeName` varchar(100) default NULL,
  `attributeCategory` smallint(6) default NULL,
  `description` varchar(1000) default NULL,
  `maxAttributeID` smallint(6) default NULL,
  `attributeIdx` int(11) default NULL,
  `graphicID` smallint(6) default NULL,
  `chargeRechargeTimeID` smallint(6) default NULL,
  `defaultValue` float default NULL,
  `published` tinyint(1) default NULL,
  `displayName` varchar(100) default NULL,
  `unitID` tinyint(3) unsigned default NULL,
  `stackable` tinyint(1) default NULL,
  `highIsGood` tinyint(1) default NULL,
  `categoryID` tinyint(3) unsigned default NULL,
  PRIMARY KEY  (`attributeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `dgmEffects`
--

DROP TABLE IF EXISTS `dgmEffects`;
CREATE TABLE `dgmEffects` (
  `effectID` smallint(6) NOT NULL,
  `effectName` varchar(400) default NULL,
  `effectCategory` smallint(6) default NULL,
  `preExpression` int(11) default NULL,
  `postExpression` int(11) default NULL,
  `description` varchar(1000) default NULL,
  `guid` varchar(60) default NULL,
  `graphicID` smallint(6) default NULL,
  `isOffensive` tinyint(1) default NULL,
  `isAssistance` tinyint(1) default NULL,
  `durationAttributeID` smallint(6) default NULL,
  `trackingSpeedAttributeID` smallint(6) default NULL,
  `dischargeAttributeID` smallint(6) default NULL,
  `rangeAttributeID` smallint(6) default NULL,
  `falloffAttributeID` int(11) default NULL,
  `disallowAutoRepeat` tinyint(1) default NULL,
  `published` tinyint(1) default NULL,
  `displayName` varchar(100) default NULL,
  `isWarpSafe` tinyint(1) default NULL,
  `rangeChance` tinyint(1) default NULL,
  `electronicChance` tinyint(1) default NULL,
  `propulsionChance` tinyint(1) default NULL,
  `distribution` tinyint(4) default NULL,
  `sfxName` varchar(20) default NULL,
  `npcUsageChanceAttributeID` int(11) default NULL,
  `npcActivationChanceAttributeID` int(11) default NULL,
  `fittingUsageChanceAttributeID` int(11) default NULL,
  PRIMARY KEY  (`effectID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `dgmTypeAttributes`
--

DROP TABLE IF EXISTS `dgmTypeAttributes`;
CREATE TABLE `dgmTypeAttributes` (
  `typeID` int(11) NOT NULL,
  `attributeID` smallint(6) NOT NULL,
  `valueInt` int(11) default NULL,
  `valueFloat` float default NULL,
  PRIMARY KEY  (`typeID`,`attributeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `dgmTypeEffects`
--

DROP TABLE IF EXISTS `dgmTypeEffects`;
CREATE TABLE `dgmTypeEffects` (
  `typeID` int(11) NOT NULL,
  `effectID` smallint(6) NOT NULL,
  `isDefault` tinyint(1) default NULL,
  PRIMARY KEY  (`typeID`,`effectID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `eveGraphics`
--

DROP TABLE IF EXISTS `eveGraphics`;
CREATE TABLE `eveGraphics` (
  `graphicID` smallint(6) NOT NULL,
  `url3D` varchar(100) default NULL,
  `urlWeb` varchar(100) default NULL,
  `description` varchar(1000) default NULL,
  `published` tinyint(1) default NULL,
  `obsolete` tinyint(1) default NULL,
  `icon` varchar(100) default NULL,
  `urlSound` varchar(100) default NULL,
  `explosionID` smallint(6) default NULL,
  PRIMARY KEY  (`graphicID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `eveNames`
--

DROP TABLE IF EXISTS `eveNames`;
CREATE TABLE `eveNames` (
  `itemID` int(11) NOT NULL,
  `itemName` varchar(100) character set utf8 default NULL,
  `categoryID` tinyint(3) unsigned default NULL,
  `groupID` tinyint(3) unsigned default NULL,
  `typeID` smallint(6) default NULL,
  PRIMARY KEY  (`itemID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `eveUnits`
--

DROP TABLE IF EXISTS `eveUnits`;
CREATE TABLE `eveUnits` (
  `unitID` tinyint(3) unsigned NOT NULL,
  `unitName` varchar(100) default NULL,
  `displayName` varchar(20) default NULL,
  `description` varchar(1000) default NULL,
  PRIMARY KEY  (`unitID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `invBlueprintTypes`
--

DROP TABLE IF EXISTS `invBlueprintTypes`;
CREATE TABLE `invBlueprintTypes` (
  `blueprintTypeID` int(11) NOT NULL,
  `parentBlueprintTypeID` int(11) default NULL,
  `productTypeID` int(11) default NULL,
  `productionTime` int(11) default NULL,
  `techLevel` smallint(6) default NULL,
  `researchProductivityTime` int(11) default NULL,
  `researchMaterialTime` int(11) default NULL,
  `researchCopyTime` int(11) default NULL,
  `researchTechTime` int(11) default NULL,
  `productivityModifier` int(11) default NULL,
  `materialModifier` smallint(6) default NULL,
  `wasteFactor` smallint(6) default NULL,
  `chanceOfReverseEngineering` float default NULL,
  `maxProductionLimit` int(11) default NULL,
  PRIMARY KEY  (`blueprintTypeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `invCategories`
--

DROP TABLE IF EXISTS `invCategories`;
CREATE TABLE `invCategories` (
  `categoryID` tinyint(3) unsigned NOT NULL,
  `categoryName` varchar(100) character set utf8 default NULL,
  `description` varchar(3000) character set utf8 default NULL,
  `graphicID` smallint(6) default NULL,
  `published` tinyint(1) default NULL,
  PRIMARY KEY  (`categoryID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `invContrabandTypes`
--

DROP TABLE IF EXISTS `invContrabandTypes`;
CREATE TABLE `invContrabandTypes` (
  `factionID` int(11) NOT NULL,
  `typeID` int(11) NOT NULL,
  `standingLoss` float default NULL,
  `confiscateMinSec` float default NULL,
  `fineByValue` float default NULL,
  `attackMinSec` float default NULL,
  PRIMARY KEY  (`factionID`,`typeID`),
  KEY `invContrabandTypes_IX_type` (`typeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `invControlTowerResourcePurposes`
--

DROP TABLE IF EXISTS `invControlTowerResourcePurposes`;
CREATE TABLE `invControlTowerResourcePurposes` (
  `purpose` tinyint(4) NOT NULL,
  `purposeText` varchar(100) default NULL,
  PRIMARY KEY  (`purpose`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `invControlTowerResources`
--

DROP TABLE IF EXISTS `invControlTowerResources`;
CREATE TABLE `invControlTowerResources` (
  `controlTowerTypeID` int(11) NOT NULL,
  `resourceTypeID` int(11) NOT NULL,
  `purpose` tinyint(4) default NULL,
  `quantity` int(11) default NULL,
  `minSecurityLevel` float default NULL,
  `factionID` int(11) default NULL,
  PRIMARY KEY  (`controlTowerTypeID`,`resourceTypeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `invFlags`
--

DROP TABLE IF EXISTS `invFlags`;
CREATE TABLE `invFlags` (
  `flagID` tinyint(3) unsigned NOT NULL,
  `flagName` varchar(100) default NULL,
  `flagText` varchar(100) default NULL,
  `flagType` varchar(20) default NULL,
  `orderID` smallint(6) default NULL,
  PRIMARY KEY  (`flagID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `invGroups`
--

DROP TABLE IF EXISTS `invGroups`;
CREATE TABLE `invGroups` (
  `groupID` smallint(6) NOT NULL,
  `categoryID` tinyint(3) unsigned default NULL,
  `groupName` varchar(100) character set utf8 default NULL,
  `description` varchar(3000) character set utf8 default NULL,
  `graphicID` smallint(6) default NULL,
  `useBasePrice` tinyint(1) default NULL,
  `allowManufacture` tinyint(1) default NULL,
  `allowRecycler` tinyint(1) default NULL,
  `anchored` tinyint(1) default NULL,
  `anchorable` tinyint(1) default NULL,
  `fittableNonSingleton` tinyint(1) default NULL,
  `published` tinyint(1) default NULL,
  PRIMARY KEY  (`groupID`),
  KEY `invGroups_IX_category` (`categoryID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `invMarketGroups`
--

DROP TABLE IF EXISTS `invMarketGroups`;
CREATE TABLE `invMarketGroups` (
  `marketGroupID` smallint(6) NOT NULL,
  `parentGroupID` smallint(6) default NULL,
  `marketGroupName` varchar(100) character set utf8 default NULL,
  `description` varchar(3000) character set utf8 default NULL,
  `graphicID` smallint(6) default NULL,
  `hasTypes` tinyint(1) default NULL,
  PRIMARY KEY  (`marketGroupID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `invMetaGroups`
--

DROP TABLE IF EXISTS `invMetaGroups`;
CREATE TABLE `invMetaGroups` (
  `metaGroupID` smallint(6) NOT NULL,
  `metaGroupName` varchar(100) character set utf8 default NULL,
  `description` varchar(1000) character set utf8 default NULL,
  `graphicID` smallint(6) default NULL,
  PRIMARY KEY  (`metaGroupID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `invMetaTypes`
--

DROP TABLE IF EXISTS `invMetaTypes`;
CREATE TABLE `invMetaTypes` (
  `typeID` int(11) NOT NULL,
  `parentTypeID` int(11) default NULL,
  `metaGroupID` smallint(6) default NULL,
  PRIMARY KEY  (`typeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `invTypeReactions`
--

DROP TABLE IF EXISTS `invTypeReactions`;
CREATE TABLE `invTypeReactions` (
  `reactionTypeID` smallint(6) NOT NULL,
  `input` tinyint(1) NOT NULL,
  `typeID` smallint(6) NOT NULL,
  `quantity` tinyint(4) default NULL,
  PRIMARY KEY  (`reactionTypeID`,`input`,`typeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `invTypes`
--

DROP TABLE IF EXISTS `invTypes`;
CREATE TABLE `invTypes` (
  `typeID` smallint(6) NOT NULL,
  `groupID` int(11) default NULL,
  `typeName` varchar(100) character set utf8 default NULL,
  `description` varchar(3000) character set utf8 default NULL,
  `graphicID` smallint(6) default NULL,
  `radius` float default NULL,
  `mass` float default NULL,
  `volume` float default NULL,
  `capacity` float default NULL,
  `portionSize` int(11) default NULL,
  `raceID` tinyint(3) unsigned default NULL,
  `basePrice` float default NULL,
  `published` tinyint(1) default NULL,
  `marketGroupID` smallint(6) default NULL,
  `chanceOfDuplicating` float default NULL,
  PRIMARY KEY  (`typeID`),
  KEY `invTypes_IX_Group` (`groupID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `mapCelestialStatistics`
--

DROP TABLE IF EXISTS `mapCelestialStatistics`;
CREATE TABLE `mapCelestialStatistics` (
  `celestialID` int(11) NOT NULL,
  `temperature` float default NULL,
  `spectralClass` varchar(10) default NULL,
  `luminosity` float default NULL,
  `age` float default NULL,
  `life` float default NULL,
  `orbitRadius` float default NULL,
  `eccentricity` float default NULL,
  `massDust` float default NULL,
  `massGas` float default NULL,
  `fragmented` tinyint(1) default NULL,
  `density` float default NULL,
  `surfaceGravity` float default NULL,
  `escapeVelocity` float default NULL,
  `orbitPeriod` float default NULL,
  `rotationRate` float default NULL,
  `locked` tinyint(1) default NULL,
  `pressure` float default NULL,
  `radius` float default NULL,
  `mass` float default NULL,
  PRIMARY KEY  (`celestialID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `mapConstellationJumps`
--

DROP TABLE IF EXISTS `mapConstellationJumps`;
CREATE TABLE `mapConstellationJumps` (
  `fromRegionID` int(11) default NULL,
  `fromConstellationID` int(11) NOT NULL,
  `toConstellationID` int(11) NOT NULL,
  `toRegionID` int(11) default NULL,
  PRIMARY KEY  (`fromConstellationID`,`toConstellationID`),
  KEY `mapConstellationJumps_IX_fromRegion` (`fromRegionID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `mapConstellations`
--

DROP TABLE IF EXISTS `mapConstellations`;
CREATE TABLE `mapConstellations` (
  `regionID` int(11) default NULL,
  `constellationID` int(11) NOT NULL,
  `constellationName` varchar(100) character set utf8 default NULL,
  `x` float default NULL,
  `y` float default NULL,
  `z` float default NULL,
  `xMin` float default NULL,
  `xMax` float default NULL,
  `yMin` float default NULL,
  `yMax` float default NULL,
  `zMin` float default NULL,
  `zMax` float default NULL,
  `factionID` int(11) default NULL,
  `radius` float default NULL,
  PRIMARY KEY  (`constellationID`),
  KEY `mapConstellations_IX_region` (`regionID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `mapDenormalize`
--

DROP TABLE IF EXISTS `mapDenormalize`;
CREATE TABLE `mapDenormalize` (
  `itemID` int(11) NOT NULL,
  `typeID` smallint(6) default NULL,
  `groupID` tinyint(3) unsigned default NULL,
  `solarSystemID` int(11) default NULL,
  `constellationID` int(11) default NULL,
  `regionID` int(11) default NULL,
  `orbitID` int(11) default NULL,
  `x` float default NULL,
  `y` float default NULL,
  `z` float default NULL,
  `radius` float default NULL,
  `itemName` varchar(100) character set utf8 default NULL,
  `security` float default NULL,
  `celestialIndex` tinyint(4) default NULL,
  `orbitIndex` tinyint(4) default NULL,
  PRIMARY KEY  (`itemID`),
  KEY `mapDenormalize_IX_constellation` (`constellationID`),
  KEY `mapDenormalize_IX_groupConstellation` (`groupID`,`constellationID`),
  KEY `mapDenormalize_IX_groupRegion` (`groupID`,`regionID`),
  KEY `mapDenormalize_IX_groupSystem` (`groupID`,`solarSystemID`),
  KEY `mapDenormalize_IX_orbit` (`orbitID`),
  KEY `mapDenormalize_IX_region` (`regionID`),
  KEY `mapDenormalize_IX_system` (`solarSystemID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `mapJumps`
--

DROP TABLE IF EXISTS `mapJumps`;
CREATE TABLE `mapJumps` (
  `stargateID` int(11) NOT NULL,
  `celestialID` int(11) default NULL,
  PRIMARY KEY  (`stargateID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `mapLandmarks`
--

DROP TABLE IF EXISTS `mapLandmarks`;
CREATE TABLE `mapLandmarks` (
  `landmarkID` smallint(6) NOT NULL,
  `landmarkName` varchar(100) default NULL,
  `description` varchar(7000) default NULL,
  `locationID` int(11) default NULL,
  `x` float default NULL,
  `y` float default NULL,
  `z` float default NULL,
  `radius` float default NULL,
  `graphicID` smallint(6) default NULL,
  `importance` tinyint(4) default NULL,
  `url2d` varchar(255) default NULL,
  PRIMARY KEY  (`landmarkID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `mapRegionJumps`
--

DROP TABLE IF EXISTS `mapRegionJumps`;
CREATE TABLE `mapRegionJumps` (
  `fromRegionID` int(11) NOT NULL,
  `toRegionID` int(11) NOT NULL,
  PRIMARY KEY  (`fromRegionID`,`toRegionID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `mapRegions`
--

DROP TABLE IF EXISTS `mapRegions`;
CREATE TABLE `mapRegions` (
  `regionID` int(11) NOT NULL,
  `regionName` varchar(100) character set utf8 default NULL,
  `x` float default NULL,
  `y` float default NULL,
  `z` float default NULL,
  `xMin` float default NULL,
  `xMax` float default NULL,
  `yMin` float default NULL,
  `yMax` float default NULL,
  `zMin` float default NULL,
  `zMax` float default NULL,
  `factionID` int(11) default NULL,
  `radius` float default NULL,
  PRIMARY KEY  (`regionID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `mapSolarSystemJumps`
--

DROP TABLE IF EXISTS `mapSolarSystemJumps`;
CREATE TABLE `mapSolarSystemJumps` (
  `fromRegionID` int(11) default NULL,
  `fromConstellationID` int(11) default NULL,
  `fromSolarSystemID` int(11) NOT NULL,
  `toSolarSystemID` int(11) NOT NULL,
  `toConstellationID` int(11) default NULL,
  `toRegionID` int(11) default NULL,
  PRIMARY KEY  (`fromSolarSystemID`,`toSolarSystemID`),
  KEY `mapSolarSystemJumps_IX_fromConstellation` (`fromConstellationID`),
  KEY `mapSolarSystemJumps_IX_fromRegion` (`fromRegionID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `mapSolarSystems`
--

DROP TABLE IF EXISTS `mapSolarSystems`;
CREATE TABLE `mapSolarSystems` (
  `regionID` int(11) default NULL,
  `constellationID` int(11) default NULL,
  `solarSystemID` int(11) NOT NULL,
  `solarSystemName` varchar(100) character set utf8 default NULL,
  `x` float default NULL,
  `y` float default NULL,
  `z` float default NULL,
  `xMin` float default NULL,
  `xMax` float default NULL,
  `yMin` float default NULL,
  `yMax` float default NULL,
  `zMin` float default NULL,
  `zMax` float default NULL,
  `luminosity` float default NULL,
  `border` tinyint(1) default NULL,
  `fringe` tinyint(1) default NULL,
  `corridor` tinyint(1) default NULL,
  `hub` tinyint(1) default NULL,
  `international` tinyint(1) default NULL,
  `regional` tinyint(1) default NULL,
  `constellation` tinyint(1) default NULL,
  `security` float default NULL,
  `factionID` int(11) default NULL,
  `radius` float default NULL,
  `sunTypeID` smallint(6) default NULL,
  `securityClass` varchar(2) default NULL,
  PRIMARY KEY  (`solarSystemID`),
  KEY `mapSolarSystems_IX_constellation` (`constellationID`),
  KEY `mapSolarSystems_IX_region` (`regionID`),
  KEY `mapSolarSystems_IX_security` (`security`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `mapUniverse`
--

DROP TABLE IF EXISTS `mapUniverse`;
CREATE TABLE `mapUniverse` (
  `universeID` int(11) NOT NULL,
  `universeName` varchar(100) default NULL,
  `x` float default NULL,
  `y` float default NULL,
  `z` float default NULL,
  `xMin` float default NULL,
  `xMax` float default NULL,
  `yMin` float default NULL,
  `yMax` float default NULL,
  `zMin` float default NULL,
  `zMax` float default NULL,
  `radius` float default NULL,
  PRIMARY KEY  (`universeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `ramActivities`
--

DROP TABLE IF EXISTS `ramActivities`;
CREATE TABLE `ramActivities` (
  `activityID` tinyint(3) unsigned NOT NULL,
  `activityName` varchar(100) character set utf8 default NULL,
  `iconNo` varchar(5) default NULL,
  `description` varchar(1000) character set utf8 default NULL,
  `published` tinyint(1) default NULL,
  PRIMARY KEY  (`activityID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `ramAssemblyLineStations`
--

DROP TABLE IF EXISTS `ramAssemblyLineStations`;
CREATE TABLE `ramAssemblyLineStations` (
  `stationID` int(11) NOT NULL,
  `assemblyLineTypeID` tinyint(3) unsigned NOT NULL,
  `quantity` tinyint(4) default NULL,
  `stationTypeID` smallint(6) default NULL,
  `ownerID` int(11) default NULL,
  `solarSystemID` int(11) default NULL,
  `regionID` int(11) default NULL,
  PRIMARY KEY  (`stationID`,`assemblyLineTypeID`),
  KEY `ramAssemblyLineStations_IX_owner` (`ownerID`),
  KEY `ramAssemblyLineStations_IX_region` (`regionID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `ramAssemblyLineTypeDetailPerCategory`
--

DROP TABLE IF EXISTS `ramAssemblyLineTypeDetailPerCategory`;
CREATE TABLE `ramAssemblyLineTypeDetailPerCategory` (
  `assemblyLineTypeID` tinyint(3) unsigned NOT NULL,
  `categoryID` tinyint(3) unsigned NOT NULL,
  `timeMultiplier` float default NULL,
  `materialMultiplier` float default NULL,
  PRIMARY KEY  (`assemblyLineTypeID`,`categoryID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `ramAssemblyLineTypeDetailPerGroup`
--

DROP TABLE IF EXISTS `ramAssemblyLineTypeDetailPerGroup`;
CREATE TABLE `ramAssemblyLineTypeDetailPerGroup` (
  `assemblyLineTypeID` tinyint(3) unsigned NOT NULL,
  `groupID` smallint(6) NOT NULL,
  `timeMultiplier` float default NULL,
  `materialMultiplier` float default NULL,
  PRIMARY KEY  (`assemblyLineTypeID`,`groupID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `ramAssemblyLineTypes`
--

DROP TABLE IF EXISTS `ramAssemblyLineTypes`;
CREATE TABLE `ramAssemblyLineTypes` (
  `assemblyLineTypeID` tinyint(3) unsigned NOT NULL,
  `assemblyLineTypeName` varchar(100) character set utf8 default NULL,
  `description` varchar(1000) character set utf8 default NULL,
  `baseTimeMultiplier` float default NULL,
  `baseMaterialMultiplier` float default NULL,
  `volume` float default NULL,
  `activityID` tinyint(3) unsigned default NULL,
  `minCostPerHour` float default NULL,
  PRIMARY KEY  (`assemblyLineTypeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `ramAssemblyLines`
--

DROP TABLE IF EXISTS `ramAssemblyLines`;
CREATE TABLE `ramAssemblyLines` (
  `assemblyLineID` int(11) NOT NULL,
  `assemblyLineTypeID` tinyint(3) unsigned default NULL,
  `containerID` int(11) default NULL,
  `nextFreeTime` datetime default NULL,
  `UIGroupingID` tinyint(3) unsigned default NULL,
  `costInstall` float default NULL,
  `costPerHour` float default NULL,
  `restrictionMask` tinyint(4) default NULL,
  `discountPerGoodStandingPoint` float default NULL,
  `surchargePerBadStandingPoint` float default NULL,
  `minimumStanding` float default NULL,
  `minimumCharSecurity` float default NULL,
  `minimumCorpSecurity` float default NULL,
  `maximumCharSecurity` float default NULL,
  `maximumCorpSecurity` float default NULL,
  `ownerID` int(11) default NULL,
  `activityID` tinyint(3) unsigned default NULL,
  PRIMARY KEY  (`assemblyLineID`),
  KEY `ramAssemblyLines_IX_container` (`containerID`),
  KEY `ramAssemblyLines_IX_owner` (`ownerID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `staOperationServices`
--

DROP TABLE IF EXISTS `staOperationServices`;
CREATE TABLE `staOperationServices` (
  `operationID` tinyint(3) unsigned NOT NULL,
  `serviceID` int(11) NOT NULL,
  PRIMARY KEY  (`operationID`,`serviceID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `staOperations`
--

DROP TABLE IF EXISTS `staOperations`;
CREATE TABLE `staOperations` (
  `activityID` tinyint(3) unsigned default NULL,
  `operationID` tinyint(3) unsigned NOT NULL,
  `operationName` varchar(100) character set utf8 default NULL,
  `description` varchar(1000) character set utf8 default NULL,
  `fringe` tinyint(4) default NULL,
  `corridor` tinyint(4) default NULL,
  `hub` tinyint(4) default NULL,
  `border` tinyint(4) default NULL,
  `ratio` tinyint(4) default NULL,
  `caldariStationTypeID` smallint(6) default NULL,
  `minmatarStationTypeID` smallint(6) default NULL,
  `amarrStationTypeID` smallint(6) default NULL,
  `gallenteStationTypeID` smallint(6) default NULL,
  `joveStationTypeID` smallint(6) default NULL,
  PRIMARY KEY  (`operationID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `staServices`
--

DROP TABLE IF EXISTS `staServices`;
CREATE TABLE `staServices` (
  `serviceID` int(11) NOT NULL,
  `serviceName` varchar(100) character set utf8 default NULL,
  `description` varchar(1000) character set utf8 default NULL,
  PRIMARY KEY  (`serviceID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `staStationTypes`
--

DROP TABLE IF EXISTS `staStationTypes`;
CREATE TABLE `staStationTypes` (
  `stationTypeID` int(11) NOT NULL,
  `dockingBayGraphicID` smallint(6) default NULL,
  `hangarGraphicID` smallint(6) default NULL,
  `dockEntryX` float default NULL,
  `dockEntryY` float default NULL,
  `dockEntryZ` float default NULL,
  `dockOrientationX` float default NULL,
  `dockOrientationY` float default NULL,
  `dockOrientationZ` float default NULL,
  `operationID` tinyint(3) unsigned default NULL,
  `officeSlots` tinyint(4) default NULL,
  `reprocessingEfficiency` float default NULL,
  `conquerable` tinyint(1) default NULL,
  PRIMARY KEY  (`stationTypeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `staStations`
--

DROP TABLE IF EXISTS `staStations`;
CREATE TABLE `staStations` (
  `stationID` int(11) NOT NULL,
  `security` smallint(6) default NULL,
  `dockingCostPerVolume` float default NULL,
  `maxShipVolumeDockable` float default NULL,
  `officeRentalCost` int(11) default NULL,
  `operationID` tinyint(3) unsigned default NULL,
  `stationTypeID` smallint(6) default NULL,
  `corporationID` int(11) default NULL,
  `solarSystemID` int(11) default NULL,
  `constellationID` int(11) default NULL,
  `regionID` int(11) default NULL,
  `stationName` varchar(100) character set utf8 default NULL,
  `x` float default NULL,
  `y` float default NULL,
  `z` float default NULL,
  `reprocessingEfficiency` float default NULL,
  `reprocessingStationsTake` float default NULL,
  `reprocessingHangarFlag` tinyint(4) default NULL,
  PRIMARY KEY  (`stationID`),
  KEY `staStations_IX_constellation` (`constellationID`),
  KEY `staStations_IX_corporation` (`corporationID`),
  KEY `staStations_IX_operation` (`operationID`),
  KEY `staStations_IX_region` (`regionID`),
  KEY `staStations_IX_system` (`solarSystemID`),
  KEY `staStations_IX_type` (`stationTypeID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;

--
-- Table structure for table `ramTypeRequirements`
--

DROP TABLE IF EXISTS `ramTypeRequirements`;
CREATE TABLE `ramTypeRequirements` (
  `typeID` smallint(6) NOT NULL,
  `activityID` tinyint(3) unsigned NOT NULL,
  `requiredTypeID` smallint(6) NOT NULL,
  `quantity` int(11) default NULL,
  `damagePerJob` float default NULL,
  PRIMARY KEY  (`typeID`,`activityID`,`requiredTypeID`),
  KEY `ramTypeRequirements_IX_activity` (`activityID`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

