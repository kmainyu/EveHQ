CREATE TABLE dbo.staOperations
(
  activityID             integer         NOT NULL,
  operationID            integer         NOT NULL,
  operationName          text(100)       NOT NULL,
  description            memo		     NOT NULL,
  fringe                 integer         NOT NULL DEFAULT 0,
  corridor               integer         NOT NULL DEFAULT 0,
  hub                    integer         NOT NULL DEFAULT 0,
  border                 integer         NOT NULL DEFAULT 0,
  ratio                  integer         NOT NULL DEFAULT 0,
  caldariStationTypeID   integer        NULL,
  minmatarStationTypeID  integer        NULL,
  amarrStationTypeID     integer        NULL,
  gallenteStationTypeID  integer        NULL,
  joveStationTypeID      integer        NULL,
  CONSTRAINT pk_staOperations PRIMARY KEY (operationID)
);

CREATE TABLE dbo.staServices
(
  serviceID      integer             NOT NULL,
  serviceName    text(100)			 NOT NULL,
  description    memo			 NOT NULL,
  CONSTRAINT pk_staServices PRIMARY KEY (serviceID)
);

CREATE TABLE dbo.staOperationServices
(
  operationID  integer  NOT NULL,
  serviceID    integer  NOT NULL
);

CREATE TABLE dbo.staStationTypes
(
  stationTypeID           integer  NOT NULL,
  dockingBayGraphicID     integer  NULL,
  hangarGraphicID         integer  NULL,
  dockEntryX              double     NOT NULL DEFAULT 0.0,
  dockEntryY              double     NOT NULL DEFAULT 0.0,
  dockEntryZ              double     NOT NULL DEFAULT 0.0,
  dockOrientationX        double     NOT NULL DEFAULT 0.0,
  dockOrientationY        double     NOT NULL DEFAULT 0.0,
  dockOrientationZ        double     NOT NULL DEFAULT -1.0,
  operationID             integer   NULL,
  officeSlots             integer   NULL,
  reprocessingEfficiency  double     NULL,
  conquerable             integer       NOT NULL DEFAULT 0,
  CONSTRAINT pk_staStationTypes PRIMARY KEY  (stationTypeID)
);

CREATE TABLE dbo.staStations
(
  stationID                 integer            NOT NULL,
  security                  integer       NOT NULL DEFAULT 500,
  dockingCostPerVolume      double          NOT NULL DEFAULT 0.0,
  maxShipVolumeDockable     double          NOT NULL DEFAULT 1150000.0,
  officeRentalCost          integer            NOT NULL DEFAULT 0,
  operationID               integer        NULL,
  stationTypeID             integer       NULL,
  corporationID             integer            NULL,
  solarSystemID             integer            NULL,
  constellationID           integer            NULL,
  regionID                  integer            NULL,
  stationName               text(100)		NOT NULL,
  x                         double          NOT NULL DEFAULT 0.0,
  y                         double          NOT NULL DEFAULT 0.0,
  z                         double          NOT NULL DEFAULT 0.0,
  reprocessingEfficiency    double          NOT NULL DEFAULT 0.5,
  reprocessingStationsTake  double          NOT NULL DEFAULT 0.025,
  reprocessingHangarFlag    integer        NOT NULL DEFAULT 4,
  CONSTRAINT pk_staStations PRIMARY KEY (stationID)
);

CREATE TABLE dbo.ramActivities
(
  activityID     integer         NOT NULL,
  activityName   text(100)	     NOT NULL,
  iconNo         text(5)         NULL,
  description    memo		     NOT NULL,
  published      integer         NOT NULL,
  CONSTRAINT pk_ramActivities PRIMARY KEY  (activityID)
);

CREATE TABLE dbo.ramAssemblyLineTypes
(
  assemblyLineTypeID      integer         NOT NULL,
  assemblyLineTypeName    text(100)		  NOT NULL,
  description             memo			  NOT NULL,
  baseTimeMultiplier      double           NOT NULL,
  baseMaterialMultiplier  double           NOT NULL,
  volume                  double           NOT NULL,
  activityID              integer         NOT NULL DEFAULT 0,
  minCostPerHour          double           NULL,
  CONSTRAINT pk_ramAssemblyLineTypes PRIMARY KEY  (assemblyLineTypeID)
);

CREATE TABLE dbo.ramAssemblyLineTypeDetailPerCategory
(
  assemblyLineTypeID  integer  NOT NULL,
  categoryID          integer  NOT NULL,
  timeMultiplier      double    NOT NULL,
  materialMultiplier  double    NOT NULL
);

CREATE TABLE dbo.ramAssemblyLineTypeDetailPerGroup
(
  assemblyLineTypeID  integer   NOT NULL,
  groupID             integer  NOT NULL,
  timeMultiplier      double     NOT NULL,
  materialMultiplier  double     NOT NULL
);

CREATE TABLE dbo.ramInstallationTypeDefaultContents
(
  installationTypeID            integer  NOT NULL,
  assemblyLineTypeID            integer   NOT NULL,
  UIGroupingID                  integer   NOT NULL,
  quantity                      integer   NOT NULL,
  costInstall                   double     NOT NULL,
  costPerHour                   double     NOT NULL,
  restrictionMask               integer   NOT NULL,
  discountPerGoodStandingPoint  double     NOT NULL,
  surchargePerBadStandingPoint  double     NOT NULL,
  minimumStanding               double     NOT NULL,
  minimumCharSecurity           double     NOT NULL,
  minimumCorpSecurity           double     NOT NULL,
  maximumCharSecurity           double     NOT NULL,
  maximumCorpSecurity           double     NOT NULL
);

CREATE TABLE dbo.ramAssemblyLines
(
  assemblyLineID                integer            NOT NULL,
  assemblyLineTypeID            integer        NOT NULL,
  containerID                   integer            NULL,
  nextFreeTime                  memo  NULL,
  UIGroupingID                  integer        NOT NULL,
  costInstall                   double          NOT NULL,
  costPerHour                   double          NOT NULL,
  restrictionMask               integer        NOT NULL,
  discountPerGoodStandingPoint  double          NOT NULL,
  surchargePerBadStandingPoint  double          NOT NULL,
  minimumStanding               double          NOT NULL,
  minimumCharSecurity           double          NOT NULL,
  minimumCorpSecurity           double          NOT NULL,
  maximumCharSecurity           double          NOT NULL,
  maximumCorpSecurity           double          NOT NULL,
  ownerID                       integer            NULL,
  activityID                    integer        NULL,
  CONSTRAINT pk_ramAssemblyLines PRIMARY KEY  (assemblyLineID)
);

CREATE TABLE dbo.ramAssemblyLineStations
(
  stationID           integer       NOT NULL,
  assemblyLineTypeID  integer   NOT NULL,
  quantity            integer   NOT NULL,
  stationTypeID       integer  NOT NULL,
  ownerID             integer       NOT NULL,
  solarSystemID       integer       NOT NULL,
  regionID            integer       NOT NULL
);

CREATE TABLE dbo.ramAssemblyLineStationCostLogs
(
  stationID           integer            NOT NULL,
  assemblyLineTypeID  integer        NOT NULL,
  logDateTime         memo			 NOT NULL,
  `usage`             double           NOT NULL,
  costPerHour         double          NOT NULL
);

CREATE TABLE dbo.ramCompletedStatuses
(
  completedStatus      integer         NOT NULL,
  completedStatusText  text(100)	   NOT NULL,
  description          memo			   NOT NULL,
  CONSTRAINT pk_ramCompletedStatuses PRIMARY KEY  (completedStatus)
);

CREATE TABLE dbo.mapUniverse
(
  universeID                    integer             NOT NULL,
  universeName                  text(100)	     NOT NULL,
  x                             double           NOT NULL DEFAULT 0.0,
  y                             double           NOT NULL DEFAULT 0.0,
  z                             double           NOT NULL DEFAULT 0.0,
  xMin                          double           NOT NULL DEFAULT 0.0,
  xMax                          double           NOT NULL DEFAULT 0.0,
  yMin                          double           NOT NULL DEFAULT 0.0,
  yMax                          double           NOT NULL DEFAULT 0.0,
  zMin                          double           NOT NULL DEFAULT 0.0,
  zMax                          double           NOT NULL DEFAULT 0.0,
  radius                        double           NOT NULL DEFAULT 0.0,
  CONSTRAINT pk_mapUniverse PRIMARY KEY  (universeID)
);


CREATE TABLE dbo.mapRegions
(
  regionID                      integer             NOT NULL,
  regionName                    text(100)	     NOT NULL,
  x                             double           NOT NULL DEFAULT 0.0,
  y                             double           NOT NULL DEFAULT 0.0,
  z                             double           NOT NULL DEFAULT 0.0,
  xMin                          double           NOT NULL DEFAULT 0.0,
  xMax                          double           NOT NULL DEFAULT 0.0,
  yMin                          double           NOT NULL DEFAULT 0.0,
  yMax                          double           NOT NULL DEFAULT 0.0,
  zMin                          double           NOT NULL DEFAULT 0.0,
  zMax                          double           NOT NULL DEFAULT 0.0,
  factionID                     integer             NULL,
  radius                        double           NOT NULL DEFAULT 0.0,
  CONSTRAINT pk_mapRegions PRIMARY KEY  (regionID)
);

CREATE TABLE dbo.mapRegionJumps
(
  fromRegionID                  integer             NOT NULL,
  toRegionID                    integer             NOT NULL
);

CREATE TABLE dbo.mapConstellations
(
  regionID                      integer             NOT NULL,
  constellationID               integer             NOT NULL,
  constellationName             text(100)	     NOT NULL,
  x                             double           NOT NULL DEFAULT 0.0,
  y                             double           NOT NULL DEFAULT 0.0,
  z                             double           NOT NULL DEFAULT 0.0,
  xMin                          double           NOT NULL DEFAULT 0.0,
  xMax                          double           NOT NULL DEFAULT 0.0,
  yMin                          double           NOT NULL DEFAULT 0.0,
  yMax                          double           NOT NULL DEFAULT 0.0,
  zMin                          double           NOT NULL DEFAULT 0.0,
  zMax                          double           NOT NULL DEFAULT 0.0,
  factionID                     integer             NULL,
  radius                        double           NOT NULL DEFAULT 0.0,
  CONSTRAINT pk_mapConstellations PRIMARY KEY  (constellationID)
);


CREATE TABLE dbo.mapConstellationJumps
(
  fromRegionID                  integer             NOT NULL,
  fromConstellationID           integer             NOT NULL,
  toConstellationID             integer             NOT NULL,
  toRegionID                    integer             NOT NULL
);


CREATE TABLE dbo.mapSolarSystems
(
  regionID                      integer             NOT NULL,
  constellationID               integer             NOT NULL,
  solarSystemID                 integer             NOT NULL,
  solarSystemName               text(100)	     NOT NULL,
  x                             double           NOT NULL DEFAULT 0.0,
  y                             double           NOT NULL DEFAULT 0.0,
  z                             double           NOT NULL DEFAULT 0.0,
  xMin                          double           NOT NULL DEFAULT 0.0,
  xMax                          double           NOT NULL DEFAULT 0.0,
  yMin                          double           NOT NULL DEFAULT 0.0,
  yMax                          double           NOT NULL DEFAULT 0.0,
  zMin                          double           NOT NULL DEFAULT 0.0,
  zMax                          double           NOT NULL DEFAULT 0.0,
  luminosity                    double           NOT NULL DEFAULT 0.0,
  border                        integer             NOT NULL DEFAULT 0,
  fringe                        integer             NOT NULL DEFAULT 0,
  corridor                      integer             NOT NULL DEFAULT 0,
  hub                           integer             NOT NULL DEFAULT 0,
  international                 integer             NOT NULL DEFAULT 0,
  regional                      integer             NOT NULL DEFAULT 0,
  constellation                 integer             NOT NULL DEFAULT 0,
  security                      double           NOT NULL DEFAULT 0.0,
  factionID                     integer             NULL,
  radius                        double           NOT NULL DEFAULT 0.0,
  sunTypeID                     integer        NULL,
  securityClass                 text(2)	      NULL,
  CONSTRAINT pk_mapSolarSystems PRIMARY KEY  (solarSystemID)
);


CREATE TABLE dbo.mapSolarSystemJumps
(
  fromRegionID                  integer             NOT NULL,
  fromConstellationID           integer             NOT NULL,
  fromSolarSystemID             integer             NOT NULL,
  toSolarSystemID               integer             NOT NULL,
  toConstellationID             integer             NOT NULL,
  toRegionID                    integer             NOT NULL
);


CREATE TABLE dbo.mapDenormalize
(
  itemID                        integer             NOT NULL,
  typeID                        integer        NOT NULL,
  groupID                       integer         NOT NULL,
  solarSystemID                 integer             NULL,
  constellationID               integer             NULL,
  regionID                      integer             NULL,
  orbitID                       integer             NULL,
  x                             double           NULL,
  y                             double           NULL,
  z                             double           NULL,
  radius                        double           NULL,
  itemName                      text(100)		 NULL,
  security                      double           NULL,
  celestialIndex                integer         NULL,
  orbitIndex                    integer         NULL,
  CONSTRAINT pk_mapDenormalize PRIMARY KEY  (itemID)
);

CREATE TABLE dbo.mapLandmarks
(
  landmarkID                    integer        NOT NULL,
  landmarkName                  text(100)       NOT NULL,
  description                   memo		   NOT NULL,
  locationID                    integer             NULL,
  x                             double           NOT NULL DEFAULT 0.0,
  y                             double           NOT NULL DEFAULT 0.0,
  z                             double           NOT NULL DEFAULT 0.0,
  radius                        double           NOT NULL DEFAULT 0.0,
  graphicID                     integer        NULL,
  importance                    integer         NOT NULL DEFAULT 0,
  url2d                         text(255)       NULL,
  CONSTRAINT pk_mapLandmarks PRIMARY KEY  (landmarkID)
);

CREATE TABLE dbo.mapSecurityRatings
(
  fromSolarSystemID             integer             NOT NULL,
  fromValue                     double           NOT NULL DEFAULT 0.0,
  toSolarSystemID               integer             NOT NULL,
  toValue                       double           NOT NULL DEFAULT 0.0
);

CREATE TABLE dbo.mapCelestialStatistics
(
  celestialID                   integer             NOT NULL,
  temperature                   double           NOT NULL DEFAULT 0.0,
  spectralClass                 text(10)         NOT NULL,
  luminosity                    double           NOT NULL DEFAULT 0.0,
  age                           double           NOT NULL DEFAULT 0.0,
  life                          double           NOT NULL DEFAULT 0.0,
  orbitRadius                   double           NOT NULL DEFAULT 0.0,
  eccentricity                  double           NOT NULL DEFAULT 0.0,
  massDust                      double           NOT NULL DEFAULT 0.0,
  massGas                       double           NOT NULL DEFAULT 0.0,
  fragmented                    integer             NOT NULL DEFAULT 0,
  density                       double           NOT NULL DEFAULT 0.0,
  surfaceGravity                double           NOT NULL DEFAULT 0.0,
  escapeVelocity                double           NOT NULL DEFAULT 0.0,
  orbitPeriod                   double           NOT NULL DEFAULT 0.0,
  rotationRate                  double           NOT NULL DEFAULT 0.0,
  locked                        integer             NOT NULL DEFAULT 0,
  pressure                      double           NOT NULL DEFAULT 0.0,
  radius                        double           NOT NULL DEFAULT 0.0,
  mass                          double           NOT NULL DEFAULT 0.0,
  CONSTRAINT pk_mapCelestialStatistics PRIMARY KEY  (celestialID)
);

CREATE TABLE dbo.mapJumps
(
  stargateID                    integer             NOT NULL,
  celestialID                   integer             NOT NULL,
  CONSTRAINT pk_mapJumps PRIMARY KEY  (stargateID)
);

CREATE TABLE dbo.invCategories
(
  categoryID     integer         NOT NULL,
  categoryName   text(100)		NOT NULL,
  description    memo			NOT NULL,
  graphicID      integer        NULL,
  published      integer		NOT NULL DEFAULT 0,
  CONSTRAINT pk_invCategories PRIMARY KEY  (categoryID)
);

CREATE TABLE dbo.invGroups
(
  groupID               integer        NOT NULL,
  categoryID            integer         NOT NULL DEFAULT 0,
  groupName             text(100)   NOT NULL,
  description           memo   NOT NULL,
  graphicID             integer        NULL,
  useBasePrice          integer             NOT NULL DEFAULT 0,
  allowManufacture      integer             NOT NULL DEFAULT 1,
  allowRecycler         integer             NOT NULL DEFAULT 1,
  anchored              integer             NOT NULL DEFAULT 0,
  anchorable            integer             NOT NULL DEFAULT 0,
  fittableNonSingleton  integer             NOT NULL DEFAULT 0,
  published				integer				NOT NULL DEFAULT 0,
  CONSTRAINT pk_invGroups PRIMARY KEY  (groupID)
);

CREATE TABLE dbo.invMarketGroups
(
  marketGroupID    integer        NOT NULL,
  parentGroupID    integer        NULL,
  marketGroupName  text(100)   NOT NULL,
  description      memo	  NOT NULL,
  graphicID        integer        NULL,
  hasTypes         integer             NOT NULL DEFAULT 0,
  CONSTRAINT pk_invMarketGroups PRIMARY KEY  (marketGroupID)
);

CREATE TABLE dbo.invTypes
(
  typeID               integer        NOT NULL,
  groupID              integer        NOT NULL DEFAULT 0,
  typeName             text(100)   NOT NULL,
  description          memo	  NOT NULL,
  graphicID            integer        NULL,
  radius               double           NOT NULL DEFAULT 0.0,
  mass                 double           NOT NULL DEFAULT 0.0,
  volume               double           NOT NULL DEFAULT 0.0,
  capacity             double           NOT NULL DEFAULT 0.0,
  portionSize          integer             NOT NULL DEFAULT 1,
  raceID               integer         NULL,
  basePrice            double           NOT NULL DEFAULT 0.0,
  published            integer             NOT NULL DEFAULT 1,
  marketGroupID        integer        NULL,
  chanceOfDuplicating  double           NOT NULL DEFAULT 0.0,
  CONSTRAINT pk_invTypes PRIMARY KEY  (typeID)
);

CREATE TABLE dbo.invBlueprintTypes
(
  blueprintTypeID             integer  NOT NULL,
  parentBlueprintTypeID       integer  NULL,
  productTypeID               integer  NULL,
  productionTime              integer       NOT NULL DEFAULT 0,
  techLevel                   integer  NOT NULL DEFAULT 0,
  researchProductivityTime    integer       NOT NULL DEFAULT 0,
  researchMaterialTime        integer       NOT NULL DEFAULT 0,
  researchCopyTime            integer       NOT NULL DEFAULT 0,
  researchTechTime            integer       NOT NULL DEFAULT 0,
  productivityModifier        integer       NOT NULL DEFAULT 0,
  materialModifier            integer  NOT NULL DEFAULT 0,
  wasteFactor                 integer  NOT NULL DEFAULT 100,
  chanceOfReverseEngineering  double     NOT NULL DEFAULT 0.0,
  maxProductionLimit          integer       NOT NULL DEFAULT 1000,
  CONSTRAINT pk_invBlueprintTypes PRIMARY KEY  (blueprintTypeID)
);

CREATE TABLE dbo.invControlTowerResourcePurposes
(
  purpose      integer       NOT NULL,
  purposeText  text(100)     NOT NULL,
  CONSTRAINT pk_invControlTowerResourcePurposes PRIMARY KEY  (purpose)
);

CREATE TABLE dbo.invControlTowerResources
(
  controlTowerTypeID  integer  NOT NULL,
  resourceTypeID      integer  NOT NULL,
  purpose             integer   NOT NULL,
  quantity            integer       NOT NULL,
  minSecurityLevel    double     NULL,
  factionID           integer       NULL
);

CREATE TABLE dbo.invContrabandTypes
(
  factionID         integer       NOT NULL,
  typeID            integer  NOT NULL,
  standingLoss      double     NOT NULL DEFAULT 0.0,
  confiscateMinSec  double     NOT NULL DEFAULT -1.0,
  fineByValue       double     NOT NULL DEFAULT 0.0,
  attackMinSec      double     NOT NULL DEFAULT -1.0
);

CREATE TABLE dbo.eveGraphics
(
  graphicID                     integer        NOT NULL,
  url3D                         text(100)    NOT NULL,
  urlWeb                        text(100)    NOT NULL,
  description                   memo   NOT NULL,
  published                     integer             NOT NULL DEFAULT 1,
  obsolete                      integer             NOT NULL DEFAULT 0,
  icon                          text(100)    NOT NULL,
  urlSound                      text(100)    NOT NULL,
  explosionID                   integer        NULL,
  CONSTRAINT pk_eveGraphics PRIMARY KEY  (graphicID)
);

CREATE TABLE dbo.eveNames
(
  itemID                        integer             NOT NULL,
  itemName                      text(100)	   NOT NULL,
  categoryID                    integer         NOT NULL,
  groupID                       integer         NOT NULL,
  typeID                        integer        NOT NULL,
  CONSTRAINT pk_eveNames PRIMARY KEY  (itemID)
);

CREATE TABLE dbo.dgmAttributeTypes
(
  attributeID                   integer             NOT NULL,
  attributeName                 text(100)			NOT NULL,
  attributeCategory             integer				NOT NULL,
  description                   memo				NOT NULL,
  maxAttributeID                integer             NULL,
  attributeIdx                  integer             NULL,
  graphicID                     integer				NULL,
  chargeRechargeTimeID          integer             NULL,
  defaultValue                  integer             NOT NULL DEFAULT 0,
  published                     integer             NOT NULL DEFAULT 1,
  displayName                   text(100)			NOT NULL,
  unitID                        integer				NULL,
  stackable                     integer             NOT NULL DEFAULT 1,
  highIsGood                    integer             NOT NULL DEFAULT 1,
  categoryID					integer				NULL,
  CONSTRAINT pk_dgmAttributeTypes PRIMARY KEY  (attributeID)
);

CREATE TABLE dbo.dgmAttributeCategories
(
	categoryID					integer			NOT NULL,
	categoryName				text(50)		NULL,
	categoryDescription			text(200)		NULL,
	CONSTRAINT dgmAttributeCategories_PK PRIMARY KEY  (categoryID)
);

CREATE TABLE dbo.dgmTypeAttributes
(
	typeID                      integer			NOT NULL,
	attributeID                 integer			NOT NULL,
	valueInt			        integer			NULL,
	valueFloat					double			NULL
);

CREATE TABLE dbo.dgmEffects
(
  effectID                        integer            NOT NULL,
  effectName                      memo			     NOT NULL,
  effectCategory                  integer			 NOT NULL,
  preExpression                   integer            NULL,
  postExpression                  integer            NULL,
  description                     memo				 NULL,
  `guid`                          text(60)			 NULL,
  graphicID                       integer		     NULL,
  isOffensive                     integer            NOT NULL DEFAULT 0,
  isAssistance                    integer            NOT NULL DEFAULT 0,
  durationAttributeID             integer            NULL,
  trackingSpeedAttributeID        integer            NULL,
  dischargeAttributeID            integer            NULL,
  rangeAttributeID                integer            NULL,
  falloffAttributeID              integer            NULL,
  disallowAutoRepeat              integer            NOT NULL DEFAULT 0,
  published                       integer            NOT NULL DEFAULT 1,
  displayName                     text(100)			 NOT NULL,
  isWarpSafe                      integer            NOT NULL DEFAULT 0,
  rangeChance                     integer            NOT NULL DEFAULT 0,
  electronicChance                integer            NOT NULL DEFAULT 0,
  propulsionChance                integer            NOT NULL DEFAULT 0,
  distribution                    integer		     NULL,
  sfxName                         text(20)		     NULL,
  npcUsageChanceAttributeID       integer            NULL,
  npcActivationChanceAttributeID  integer            NULL,
  fittingUsageChanceAttributeID   integer            NULL,
  CONSTRAINT pk_dgmEffects PRIMARY KEY  (effectID)
);

CREATE TABLE dbo.dgmTypeEffects
(
  typeID                        integer        NOT NULL,
  effectID                      integer             NOT NULL,
  isDefault                     integer             NOT NULL DEFAULT 0
);

CREATE TABLE dbo.crpActivities
(
  activityID                    integer         NOT NULL,
  activityName                  text(100)   NOT NULL,
  description                   memo  NOT NULL,
  CONSTRAINT pk_crpActivities PRIMARY KEY  (activityID)
);

CREATE TABLE dbo.crpNPCCorporations
(
  corporationID                 integer             NOT NULL,
  mainActivityID                integer         NOT NULL,
  secondaryActivityID           integer         NULL,
  `size`                        text(1)         NOT NULL,
  extent                        text(1)         NOT NULL,
  solarSystemID                 integer             NULL,
  investorID1                   integer             NULL,
  investorShares1               integer         NOT NULL DEFAULT 0,
  investorID2                   integer             NULL,
  investorShares2               integer         NOT NULL DEFAULT 0,
  investorID3                   integer             NULL,
  investorShares3               integer         NOT NULL DEFAULT 0,
  investorID4                   integer             NULL,
  investorShares4               integer         NOT NULL DEFAULT 0,
  friendID                      integer             NULL,
  enemyID                       integer             NULL,
  publicShares                  integer    NOT NULL DEFAULT 0,
  initialPrice                  integer             NOT NULL DEFAULT 0,
  minSecurity                   double           NOT NULL DEFAULT 0.0,
  scattered                     integer             NOT NULL DEFAULT 0,
  fringe                        integer         NOT NULL DEFAULT 0,
  corridor                      integer         NOT NULL DEFAULT 0,
  hub                           integer         NOT NULL DEFAULT 0,
  border                        integer         NOT NULL DEFAULT 0,
  factionID                     integer             NOT NULL,
  sizeFactor                    double           NOT NULL,
  stationCount                  integer        NOT NULL DEFAULT 0,
  stationSystemCount            integer        NOT NULL DEFAULT 0,
  CONSTRAINT pk_crpNPCCorporations PRIMARY KEY  (corporationID)
);

CREATE TABLE dbo.crpNPCCorporationResearchFields
(
  skillID                       integer        NOT NULL,
  corporationID                 integer        NOT NULL,
  supplierTypes                 integer        NOT NULL 
);

CREATE TABLE dbo.crpNPCCorporationTrades
(
  corporationID                 integer        NOT NULL,
  typeID						integer        NOT NULL,
  supplyDemand                  double         NOT NULL 
);

CREATE TABLE dbo.crpNPCDivisions
(
  divisionID                    integer         NOT NULL,
  divisionName                  text(100)   NOT NULL,
  description                   memo  NOT NULL,
  leaderType                    text(100)   NOT NULL,
  CONSTRAINT pk_crpNPCDivisions PRIMARY KEY  (divisionID)
);

CREATE TABLE dbo.crpNPCCorporationDivisions
(
  corporationID                 integer             NOT NULL,
  divisionID                    integer         NOT NULL,
  divisionNumber                integer         NOT NULL,
  `size`                        integer         NULL,
  leaderID                      integer             NULL
);

CREATE TABLE dbo.chrAttributes
(
  attributeID       integer         NOT NULL,
  attributeName     text(100)	    NOT NULL,
  description       memo   NOT NULL,
  graphicID         integer        NULL,
  shortDescription  memo   NOT NULL,
  notes             memo   NOT NULL,
  CONSTRAINT pk_chrAttributes PRIMARY KEY  (attributeID)
);

CREATE TABLE dbo.chrRaces
(
  raceID             integer         NOT NULL,
  raceName           text(100)	   NOT NULL,
  description        memo  NOT NULL,
  graphicID          integer        NULL,
  shortDescription   memo   NOT NULL,
  CONSTRAINT pk_chrRaces PRIMARY KEY  (raceID)
);

CREATE TABLE dbo.chrBloodlines
(
  bloodlineID             integer         NOT NULL,
  bloodlineName           text(100)   NOT NULL,
  raceID                  integer         NOT NULL DEFAULT 0,
  description             memo  NOT NULL,
  maleDescription         memo  NOT NULL,
  femaleDescription       memo  NOT NULL,
  shipTypeID              integer        NOT NULL,
  corporationID           integer             NOT NULL,
  perception              integer         NOT NULL DEFAULT 0,
  willpower               integer         NOT NULL DEFAULT 0,
  charisma                integer         NOT NULL DEFAULT 0,
  memory                  integer         NOT NULL DEFAULT 0,
  intelligence            integer         NOT NULL DEFAULT 0,
  graphicID               integer        NULL,
  shortDescription        memo   NOT NULL,
  shortMaleDescription    memo   NOT NULL,
  shortFemaleDescription  memo   NOT NULL,
  CONSTRAINT pk_chrBloodlines PRIMARY KEY  (bloodlineID)
);

CREATE TABLE dbo.chrAncestries
(
  ancestryID        integer         NOT NULL,
  ancestryName      text(100)   NOT NULL,
  bloodlineID       integer         NOT NULL DEFAULT 0,
  description       memo  NOT NULL,
  perception        integer         NOT NULL DEFAULT 0,
  willpower         integer         NOT NULL DEFAULT 0,
  charisma          integer         NOT NULL DEFAULT 0,
  memory            integer         NOT NULL DEFAULT 0,
  intelligence      integer         NOT NULL DEFAULT 0,
  graphicID         integer        NULL,
  shortDescription  memo   NOT NULL,
  CONSTRAINT pk_chrAncestries PRIMARY KEY  (ancestryID)
);

CREATE TABLE dbo.chrCareers
(
  raceID            integer         NOT NULL,
  careerID          integer         NOT NULL,
  careerName        text(100)   NOT NULL,
  description       memo   NOT NULL,
  shortDescription  memo   NOT NULL,
  graphicID         integer        NULL,
  schoolID          integer         NULL,
  CONSTRAINT pk_chrCareers PRIMARY KEY  (careerID)
);

CREATE TABLE dbo.chrCareerSpecialities
(
  specialityID      integer         NOT NULL,
  careerID          integer         NOT NULL,
  specialityName    text(100)   NOT NULL,
  description       memo   NOT NULL,
  shortDescription  memo   NOT NULL,
  graphicID         integer        NULL,
  departmentID      integer         NULL,
  CONSTRAINT pk_chrCareerSpecialities PRIMARY KEY  (specialityID)
);

CREATE TABLE dbo.chrRaceSkills
(
  raceID       integer   NOT NULL,
  skillTypeID  integer  NOT NULL,
  levels       integer   NOT NULL
);

CREATE TABLE dbo.chrCareerSkills
(
  careerID     integer   NOT NULL,
  skillTypeID  integer  NOT NULL,
  levels       integer   NOT NULL
);

CREATE TABLE dbo.chrCareerSpecialitySkills
(
  specialityID  integer   NOT NULL,
  skillTypeID   integer  NOT NULL,
  levels        integer   NOT NULL
);

CREATE TABLE dbo.chrSchools
(
  raceID             integer         NOT NULL,
  schoolID           integer         NOT NULL,
  schoolName         text(100)   NOT NULL,
  description        memo  NOT NULL,
  graphicID          integer        NULL,
  corporationID      integer             NULL,
  agentID            integer             NULL,
  newAgentID         integer             NULL,
  careerID           integer         NULL,
  CONSTRAINT pk_chrSchools PRIMARY KEY  (schoolID)
);

CREATE TABLE dbo.agtAgents
(
  agentID               integer             NOT NULL,
  divisionID            integer             NULL,
  corporationID         integer             NULL,
  stationID             integer             NULL,
  `level`               integer         NULL,
  quality               integer        NULL,
  agentTypeID           integer         NOT NULL,
  CONSTRAINT pk_agtAgents PRIMARY KEY  (agentID)
);

CREATE TABLE dbo.agtConfig
(
  agentID               integer             NOT NULL,
  k                     text(50)	        NOT NULL,
  v                     memo			    NOT NULL
);

CREATE TABLE dbo.agtAgentTypes
(
  agentTypeID           integer         NOT NULL,
  agentType             text(50)        NOT NULL,
  CONSTRAINT pk_agtAgentTypes PRIMARY KEY  (agentTypeID)
);

CREATE TABLE dbo.agtResearchAgents
(
	agentID				integer				NOT NULL,
	typeID				integer				NOT NULL
 );

CREATE TABLE dbo.chrFactions
(
  factionID  		integer			NOT NULL,
  factionName  		text(100)		NOT NULL,
  description  		memo	NOT NULL,
  raceIDs  			integer				NOT NULL,
  solarSystemID  		integer			NULL,
  corporationID  		integer			NULL,
  sizeFactor  		double			NOT NULL,
  stationCount  		integer			NOT NULL,
  stationSystemCount  	integer			NOT NULL,
  militiaCorporationID	integer			NULL,
  CONSTRAINT pk_chrFactions PRIMARY KEY  (factionID)
);

CREATE TABLE dbo.chrSchoolAgents
(
  schoolID  		integer			NOT NULL,
  agentindex  		integer			NOT NULL,
  agentID	  		integer			NOT NULL,
  CONSTRAINT pk_chrSchoolAgents PRIMARY KEY  (agentID)
);

CREATE TABLE dbo.eveUnits
(
  unitID	  		integer			NOT NULL,
  unitName  		text(100)		NOT NULL,
  displayName		text(20)		NOT NULL,
  description		memo	NOT NULL,
  CONSTRAINT pk_eveUnits PRIMARY KEY  (unitID)
);

CREATE TABLE dbo.invFlags
(
  flagID	  		integer			NOT NULL,
  flagName  		text(100)		NOT NULL,
  flagText			text(100)		NOT NULL,
  flagType			text(100)		NULL,
  orderID			integer			NOT NULL,
  CONSTRAINT pk_invFlags PRIMARY KEY  (flagID)
);

CREATE TABLE dbo.invMetaGroups
(
  metaGroupID  		integer			NOT NULL,
  metaGroupName  	text(100)		NOT NULL,
  description		memo	NULL,
  graphicID			integer			NULL,
  CONSTRAINT pk_invMetaGroups PRIMARY KEY  (metaGroupID)
);

CREATE TABLE dbo.invMetaTypes
(
  typeID	  		integer			NOT NULL,
  parentTypeID  		integer			NOT NULL,
  metaGroupID		integer			NOT NULL,
  CONSTRAINT pk_invMetaTypes PRIMARY KEY  (typeID)
);

CREATE TABLE dbo.invTypeReactions
(
  reactionTypeID  	integer			NOT NULL,
  `input`  			integer			NOT NULL,
  typeID			integer			NOT NULL,
  quantity			integer			NOT NULL
);

CREATE TABLE dbo.typeActivityMaterials
(
  typeID  			integer			NOT NULL,
  activityID  		integer			NOT NULL,
  requiredTypeID	integer			NOT NULL,
  quantity			integer			NOT NULL,
  damagePerJob		double			NOT NULL
);

