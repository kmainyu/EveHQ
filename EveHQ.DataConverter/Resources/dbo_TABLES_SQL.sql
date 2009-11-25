CREATE TABLE dbo.staOperations
(
  activityID             tinyint         NOT NULL,
  operationID            tinyint         NOT NULL,
  operationName          nvarchar(100)   NOT NULL DEFAULT '',
  [description]          nvarchar(1000)  NOT NULL DEFAULT '',
  fringe                 tinyint         NOT NULL DEFAULT 0,
  corridor               tinyint         NOT NULL DEFAULT 0,
  hub                    tinyint         NOT NULL DEFAULT 0,
  border                 tinyint         NOT NULL DEFAULT 0,
  ratio                  tinyint         NOT NULL DEFAULT 0,
  caldariStationTypeID   smallint        NULL,
  minmatarStationTypeID  smallint        NULL,
  amarrStationTypeID     smallint        NULL,
  gallenteStationTypeID  smallint        NULL,
  joveStationTypeID      smallint        NULL,
  CONSTRAINT pk_staOperations PRIMARY KEY CLUSTERED (operationID)
)

CREATE TABLE dbo.staServices
(
  serviceID      int             NOT NULL,
  serviceName    nvarchar(100)   NOT NULL DEFAULT '',
  [description]  nvarchar(1000)  NOT NULL DEFAULT '',
  CONSTRAINT pk_staServices PRIMARY KEY CLUSTERED (serviceID)
)

CREATE TABLE dbo.staOperationServices
(
  operationID  tinyint  NOT NULL,
  serviceID    int      NOT NULL,
  CONSTRAINT pk_staOperationServices PRIMARY KEY CLUSTERED (operationID, serviceID)
)

CREATE TABLE dbo.staStationTypes
(
  stationTypeID           smallint  NOT NULL,
  dockingBayGraphicID     smallint  NULL,
  hangarGraphicID         smallint  NULL,
  dockEntryX              float     NOT NULL DEFAULT 0.0,
  dockEntryY              float     NOT NULL DEFAULT 0.0,
  dockEntryZ              float     NOT NULL DEFAULT 0.0,
  dockOrientationX        float     NOT NULL DEFAULT 0.0,
  dockOrientationY        float     NOT NULL DEFAULT 0.0,
  dockOrientationZ        float     NOT NULL DEFAULT -1.0,
  operationID             tinyint   NULL,
  officeSlots             tinyint   NULL,
  reprocessingEfficiency  float     NULL,
  conquerable             bit       NOT NULL DEFAULT 0,
  CONSTRAINT pk_staStationTypes PRIMARY KEY CLUSTERED (stationTypeID)
)

CREATE TABLE dbo.staStations
(
  stationID                 int            NOT NULL,
  security                  smallint       NOT NULL DEFAULT 500,
  dockingCostPerVolume      float          NOT NULL DEFAULT 0.0,
  maxShipVolumeDockable     float          NOT NULL DEFAULT 1150000.0,
  officeRentalCost          int            NOT NULL DEFAULT 0,
  operationID               tinyint        NULL,
  stationTypeID             smallint       NULL,
  corporationID             int            NULL,
  solarSystemID             int            NULL,
  constellationID           int            NULL,
  regionID                  int            NULL,
  stationName               nvarchar(100)  COLLATE Latin1_General_CI_AI NOT NULL DEFAULT '',
  x                         float          NOT NULL DEFAULT 0.0,
  y                         float          NOT NULL DEFAULT 0.0,
  z                         float          NOT NULL DEFAULT 0.0,
  reprocessingEfficiency    float          NOT NULL DEFAULT 0.5,
  reprocessingStationsTake  float          NOT NULL DEFAULT 0.025,
  reprocessingHangarFlag    tinyint        NOT NULL DEFAULT 4,
  capitalStation			smalldatetime	NULL,
  ownershipDateTime			smalldatetime	NULL,
  upgradeLevel				int				NULL,
  customServiceMask			int				NULL,
  CONSTRAINT pk_staStations PRIMARY KEY CLUSTERED (stationID)
)

CREATE TABLE dbo.ramActivities
(
  activityID     tinyint         NOT NULL,
  activityName   nvarchar(100)   NOT NULL,
  iconNo         varchar(5)      NULL,
  [description]  nvarchar(1000)  NOT NULL,
  published      bit             NOT NULL,
  CONSTRAINT pk_ramActivities PRIMARY KEY CLUSTERED (activityID)
)

CREATE TABLE dbo.ramAssemblyLineTypes
(
  assemblyLineTypeID      tinyint         NOT NULL,
  assemblyLineTypeName    nvarchar(100)   NOT NULL,
  [description]           nvarchar(1000)  NOT NULL,
  baseTimeMultiplier      float           NOT NULL,
  baseMaterialMultiplier  float           NOT NULL,
  volume                  float           NOT NULL,
  activityID              tinyint         NOT NULL DEFAULT 0,
  minCostPerHour          float           NULL,
  CONSTRAINT pk_ramAssemblyLineTypes PRIMARY KEY CLUSTERED (assemblyLineTypeID)
)

CREATE TABLE dbo.ramAssemblyLineTypeDetailPerCategory
(
  assemblyLineTypeID  tinyint  NOT NULL,
  categoryID          tinyint  NOT NULL,
  timeMultiplier      float    NOT NULL,
  materialMultiplier  float    NOT NULL,
  CONSTRAINT pk_ramAssemblyLineTypeDetailPerCategory PRIMARY KEY CLUSTERED (assemblyLineTypeID, categoryID)
)

CREATE TABLE dbo.ramAssemblyLineTypeDetailPerGroup
(
  assemblyLineTypeID  tinyint   NOT NULL,
  groupID             smallint  NOT NULL,
  timeMultiplier      float     NOT NULL,
  materialMultiplier  float     NOT NULL,
  CONSTRAINT pk_ramAssemblyLineTypeDetailPerGroup PRIMARY KEY CLUSTERED (assemblyLineTypeID, groupID)
)

CREATE TABLE dbo.ramInstallationTypeDefaultContents
(
  installationTypeID            smallint  NOT NULL,
  assemblyLineTypeID            tinyint   NOT NULL,
  UIGroupingID                  tinyint   NOT NULL,
  quantity                      tinyint   NOT NULL,
  costInstall                   float     NOT NULL,
  costPerHour                   float     NOT NULL,
  restrictionMask               tinyint   NOT NULL,
  discountPerGoodStandingPoint  float     NOT NULL,
  surchargePerBadStandingPoint  float     NOT NULL,
  minimumStanding               float     NOT NULL,
  minimumCharSecurity           float     NOT NULL,
  minimumCorpSecurity           float     NOT NULL,
  maximumCharSecurity           float     NOT NULL,
  maximumCorpSecurity           float     NOT NULL,
  CONSTRAINT pk_ramInstallationTypeDefaultContents PRIMARY KEY CLUSTERED (installationTypeID, assemblyLineTypeID, UIGroupingID)
)

CREATE TABLE dbo.ramAssemblyLines
(
  assemblyLineID                int            NOT NULL,
  assemblyLineTypeID            tinyint        NOT NULL,
  containerID                   int            NULL,
  nextFreeTime                  smalldatetime  NULL,
  UIGroupingID                  tinyint        NOT NULL,
  costInstall                   float          NOT NULL,
  costPerHour                   float          NOT NULL,
  restrictionMask               tinyint        NOT NULL,
  discountPerGoodStandingPoint  float          NOT NULL,
  surchargePerBadStandingPoint  float          NOT NULL,
  minimumStanding               float          NOT NULL,
  minimumCharSecurity           float          NOT NULL,
  minimumCorpSecurity           float          NOT NULL,
  maximumCharSecurity           float          NOT NULL,
  maximumCorpSecurity           float          NOT NULL,
  ownerID                       int            NULL,
  oldContainerID                int            NULL,
  oldOwnerID                    int            NULL,
  activityID                    tinyint        NULL,
  CONSTRAINT pk_ramAssemblyLines PRIMARY KEY CLUSTERED (assemblyLineID)
)

CREATE TABLE dbo.ramAssemblyLineStations
(
  stationID           int       NOT NULL,
  assemblyLineTypeID  tinyint   NOT NULL,
  quantity            tinyint   NOT NULL,
  stationTypeID       smallint  NOT NULL,
  ownerID             int       NOT NULL,
  solarSystemID       int       NOT NULL,
  regionID            int       NOT NULL,
  CONSTRAINT pk_ramAssemblyLineStations PRIMARY KEY CLUSTERED (stationID, assemblyLineTypeID)
)

CREATE TABLE dbo.ramAssemblyLineStationCostLogs
(
  stationID           int            NOT NULL,
  assemblyLineTypeID  tinyint        NOT NULL,
  logDateTime         smalldatetime  NOT NULL,
  usage               real           NOT NULL,
  costPerHour         float          NOT NULL,
  CONSTRAINT pk_ramAssemblyLineStationCostLogs PRIMARY KEY CLUSTERED (stationID, assemblyLineTypeID, logDateTime)
)

CREATE TABLE dbo.ramCompletedStatuses
(
  completedStatus      tinyint         NOT NULL,
  completedStatusText  nvarchar(100)   NOT NULL,
  [description]        nvarchar(1000)  NOT NULL,
  CONSTRAINT pk_ramCompletedStatuses PRIMARY KEY CLUSTERED (completedStatus)
)

CREATE TABLE dbo.mapUniverse
(
  universeID                    int             NOT NULL,
  universeName                  varchar(100)    NOT NULL DEFAULT '',
  x                             float           NOT NULL DEFAULT 0.0,
  y                             float           NOT NULL DEFAULT 0.0,
  z                             float           NOT NULL DEFAULT 0.0,
  xMin                          float           NOT NULL DEFAULT 0.0,
  xMax                          float           NOT NULL DEFAULT 0.0,
  yMin                          float           NOT NULL DEFAULT 0.0,
  yMax                          float           NOT NULL DEFAULT 0.0,
  zMin                          float           NOT NULL DEFAULT 0.0,
  zMax                          float           NOT NULL DEFAULT 0.0,
  radius                        float           NOT NULL DEFAULT 0.0,
  CONSTRAINT pk_mapUniverse PRIMARY KEY CLUSTERED (universeID)
)


CREATE TABLE dbo.mapRegions
(
  regionID                      int             NOT NULL,
  regionName                    nvarchar(100)   COLLATE Latin1_General_CI_AI NOT NULL DEFAULT '',
  x                             float           NOT NULL DEFAULT 0.0,
  y                             float           NOT NULL DEFAULT 0.0,
  z                             float           NOT NULL DEFAULT 0.0,
  xMin                          float           NOT NULL DEFAULT 0.0,
  xMax                          float           NOT NULL DEFAULT 0.0,
  yMin                          float           NOT NULL DEFAULT 0.0,
  yMax                          float           NOT NULL DEFAULT 0.0,
  zMin                          float           NOT NULL DEFAULT 0.0,
  zMax                          float           NOT NULL DEFAULT 0.0,
  factionID                     int             NULL,
  radius                        float           NOT NULL DEFAULT 0.0,
  CONSTRAINT pk_mapRegions PRIMARY KEY CLUSTERED (regionID)
)

CREATE TABLE dbo.mapRegionJumps
(
  fromRegionID                  int             NOT NULL,
  toRegionID                    int             NOT NULL,
  CONSTRAINT pk_mapRegionJumps PRIMARY KEY CLUSTERED (fromRegionID, toRegionID)
)

CREATE TABLE dbo.mapConstellations
(
  regionID                      int             NOT NULL,
  constellationID               int             NOT NULL,
  constellationName             nvarchar(100)   COLLATE Latin1_General_CI_AI NOT NULL DEFAULT '',
  x                             float           NOT NULL DEFAULT 0.0,
  y                             float           NOT NULL DEFAULT 0.0,
  z                             float           NOT NULL DEFAULT 0.0,
  xMin                          float           NOT NULL DEFAULT 0.0,
  xMax                          float           NOT NULL DEFAULT 0.0,
  yMin                          float           NOT NULL DEFAULT 0.0,
  yMax                          float           NOT NULL DEFAULT 0.0,
  zMin                          float           NOT NULL DEFAULT 0.0,
  zMax                          float           NOT NULL DEFAULT 0.0,
  factionID                     int             NULL,
  radius                        float           NOT NULL DEFAULT 0.0,
  sovereigntyDateTime			smalldatetime	NULL,
  allianceID					int				NULL,
  graceDateTime					smalldatetime	NULL,
  CONSTRAINT pk_mapConstellations PRIMARY KEY CLUSTERED (constellationID)
)


CREATE TABLE dbo.mapConstellationJumps
(
  fromRegionID                  int             NOT NULL,
  fromConstellationID           int             NOT NULL,
  toConstellationID             int             NOT NULL,
  toRegionID                    int             NOT NULL,
  CONSTRAINT pk_mapConstellationJumps PRIMARY KEY CLUSTERED (fromConstellationID, toConstellationID)
)


CREATE TABLE dbo.mapSolarSystems
(
  regionID                      int             NOT NULL,
  constellationID               int             NOT NULL,
  solarSystemID                 int             NOT NULL,
  solarSystemName               nvarchar(100)   COLLATE Latin1_General_CI_AI NOT NULL DEFAULT '',
  x                             float           NOT NULL DEFAULT 0.0,
  y                             float           NOT NULL DEFAULT 0.0,
  z                             float           NOT NULL DEFAULT 0.0,
  xMin                          float           NOT NULL DEFAULT 0.0,
  xMax                          float           NOT NULL DEFAULT 0.0,
  yMin                          float           NOT NULL DEFAULT 0.0,
  yMax                          float           NOT NULL DEFAULT 0.0,
  zMin                          float           NOT NULL DEFAULT 0.0,
  zMax                          float           NOT NULL DEFAULT 0.0,
  luminosity                    float           NOT NULL DEFAULT 0.0,
  --
  border                        bit             NOT NULL DEFAULT 0,
  fringe                        bit             NOT NULL DEFAULT 0,
  corridor                      bit             NOT NULL DEFAULT 0,
  hub                           bit             NOT NULL DEFAULT 0,
  international                 bit             NOT NULL DEFAULT 0,
  regional                      bit             NOT NULL DEFAULT 0,
  constellation                 bit             NOT NULL DEFAULT 0,
  security                      float           NOT NULL DEFAULT 0.0,
  factionID                     int             NULL,
  radius                        float           NOT NULL DEFAULT 0.0,
  sunTypeID                     smallint        NULL,
  securityClass                 varchar(2)      NULL,
  allianceID                    int             NULL,
  sovereigntyLevel				int				NULL,
  sovereigntyDateTime			smalldatetime	NULL,
  CONSTRAINT pk_mapSolarSystems PRIMARY KEY CLUSTERED (solarSystemID)
)


CREATE TABLE dbo.mapSolarSystemJumps
(
  fromRegionID                  int             NOT NULL,
  fromConstellationID           int             NOT NULL,
  fromSolarSystemID             int             NOT NULL,
  toSolarSystemID               int             NOT NULL,
  toConstellationID             int             NOT NULL,
  toRegionID                    int             NOT NULL,
  CONSTRAINT pk_mapSolarSystemJumps PRIMARY KEY CLUSTERED (fromSolarSystemID, toSolarSystemID)
)


CREATE TABLE dbo.mapDenormalize
(
  itemID                        int             NOT NULL,
  typeID                        smallint        NOT NULL,
  groupID                       tinyint         NOT NULL,
  solarSystemID                 int             NULL,
  constellationID               int             NULL,
  regionID                      int             NULL,
  orbitID                       int             NULL,
  x                             float           NULL,
  y                             float           NULL,
  z                             float           NULL,
  radius                        float           NULL,
  itemName                      nvarchar(100)   NULL,
  [security]                    float           NULL,
  celestialIndex                tinyint         NULL,
  orbitIndex                    tinyint         NULL,
  CONSTRAINT pk_mapDenormalize PRIMARY KEY CLUSTERED (itemID)
)

CREATE TABLE dbo.mapLandmarks
(
  landmarkID                    smallint        NOT NULL,
  landmarkName                  varchar(100)    NOT NULL,
  [description]                 varchar(7000)   NOT NULL DEFAULT '',
  locationID                    int             NULL,
  x                             float           NOT NULL DEFAULT 0.0,
  y                             float           NOT NULL DEFAULT 0.0,
  z                             float           NOT NULL DEFAULT 0.0,
  radius                        float           NOT NULL DEFAULT 0.0,
  graphicID                     smallint        NULL,
  importance                    tinyint         NOT NULL DEFAULT 0,
  url2d                         varchar(255)    NULL,
  CONSTRAINT pk_mapLandmarks PRIMARY KEY CLUSTERED (landmarkID)
)

CREATE TABLE dbo.mapSecurityRatings
(
  fromSolarSystemID             int             NOT NULL,
  fromValue                     float           NOT NULL DEFAULT 0.0,
  toSolarSystemID               int             NOT NULL,
  toValue                       float           NOT NULL DEFAULT 0.0,
  CONSTRAINT pk_mapSecurityRatings PRIMARY KEY CLUSTERED (fromSolarSystemID, toSolarSystemID)
)

CREATE TABLE dbo.mapCelestialStatistics
(
  celestialID                   int             NOT NULL,
  -- Star and Planet and Moon (not Fragmented)
  temperature                   float           NOT NULL DEFAULT 0.0,
  -- Star
  spectralClass                 varchar(10)     NOT NULL DEFAULT 0.0,
  luminosity                    float           NOT NULL DEFAULT 0.0,
  age                           float           NOT NULL DEFAULT 0.0,
  life                          float           NOT NULL DEFAULT 0.0,
  -- Planet and Moon
  orbitRadius                   float           NOT NULL DEFAULT 0.0,
  eccentricity                  float           NOT NULL DEFAULT 0.0,
  massDust                      float           NOT NULL DEFAULT 0.0,
  massGas                       float           NOT NULL DEFAULT 0.0,
  fragmented                    bit             NOT NULL DEFAULT 0,
  -- Planet and Moon (not Fragmented)
  density                       float           NOT NULL DEFAULT 0.0,
  surfaceGravity                float           NOT NULL DEFAULT 0.0,
  escapeVelocity                float           NOT NULL DEFAULT 0.0,
  orbitPeriod                   float           NOT NULL DEFAULT 0.0,
  rotationRate                  float           NOT NULL DEFAULT 0.0,
  locked                        bit             NOT NULL DEFAULT 0,
  pressure                      float           NOT NULL DEFAULT 0.0,
  -- Star and Planet and Moon (not Fragmented)
  radius                        float           NOT NULL DEFAULT 0.0,
  mass                          float           NOT NULL DEFAULT 0.0,
  CONSTRAINT pk_mapCelestialStatistics PRIMARY KEY CLUSTERED (celestialID)
)

CREATE TABLE dbo.mapJumps
(
  stargateID                    int             NOT NULL,
  celestialID                   int             NOT NULL,
  CONSTRAINT pk_mapJumps PRIMARY KEY CLUSTERED (stargateID)
)

CREATE TABLE dbo.invCategories
(
  categoryID     tinyint         NOT NULL,
  categoryName   nvarchar(100)   COLLATE Latin1_General_CI_AI NOT NULL DEFAULT '',
  [description]  nvarchar(3000)  NOT NULL DEFAULT '',
  graphicID      smallint        NULL,
  published      bit             NOT NULL DEFAULT 0,
  CONSTRAINT pk_invCategories PRIMARY KEY CLUSTERED (categoryID)
)

CREATE TABLE dbo.invGroups
(
  groupID               smallint        NOT NULL,
  categoryID            tinyint         NOT NULL DEFAULT 0,
  groupName             nvarchar(100)   COLLATE Latin1_General_CI_AI NOT NULL DEFAULT '',
  [description]         nvarchar(3000)  NOT NULL DEFAULT '',
  graphicID             smallint        NULL,
  useBasePrice          bit             NOT NULL DEFAULT 0,
  allowManufacture      bit             NOT NULL DEFAULT 1,
  allowRecycler         bit             NOT NULL DEFAULT 1,
  anchored              bit             NOT NULL DEFAULT 0,
  anchorable            bit             NOT NULL DEFAULT 0,
  fittableNonSingleton  bit             NOT NULL DEFAULT 0,
  published				bit             NOT NULL DEFAULT 0,
  CONSTRAINT pk_invGroups PRIMARY KEY CLUSTERED (groupID)
)

CREATE TABLE dbo.invMarketGroups
(
  marketGroupID    smallint        NOT NULL,
  parentGroupID    smallint        NULL,
  marketGroupName  nvarchar(100)   NOT NULL DEFAULT '',
  [description]    nvarchar(3000)  NOT NULL DEFAULT '',
  graphicID        smallint        NULL,
  hasTypes         bit             NOT NULL DEFAULT 0
  CONSTRAINT pk_invMarketGroups PRIMARY KEY CLUSTERED (marketGroupID)
)

CREATE TABLE dbo.invTypes
(
  typeID               smallint        NOT NULL,
  groupID              smallint        NOT NULL DEFAULT 0,
  typeName             nvarchar(100)   COLLATE Latin1_General_CI_AI NOT NULL DEFAULT '',
  [description]        nvarchar(3000)  NOT NULL DEFAULT '',
  graphicID            smallint        NULL,
  radius               float           NOT NULL DEFAULT 0.0,
  mass                 float           NOT NULL DEFAULT 0.0,
  volume               float           NOT NULL DEFAULT 0.0,
  capacity             float           NOT NULL DEFAULT 0.0,
  portionSize          int             NOT NULL DEFAULT 1,
  raceID               tinyint         NULL,
  basePrice            money           NOT NULL DEFAULT 0.0,
  published            bit             NOT NULL DEFAULT 1,
  marketGroupID        smallint        NULL,
  chanceOfDuplicating  float           NOT NULL DEFAULT 0.0,
  CONSTRAINT pk_invTypes PRIMARY KEY CLUSTERED (typeID)
)

CREATE TABLE dbo.invBlueprintTypes
(
  blueprintTypeID             smallint  NOT NULL,
  parentBlueprintTypeID       smallint  NULL,
  productTypeID               smallint  NULL,
  productionTime              int       NOT NULL DEFAULT 0,
  techLevel                   smallint  NOT NULL DEFAULT 0,
  researchProductivityTime    int       NOT NULL DEFAULT 0,
  researchMaterialTime        int       NOT NULL DEFAULT 0,
  researchCopyTime            int       NOT NULL DEFAULT 0,
  researchTechTime            int       NOT NULL DEFAULT 0,
  productivityModifier        int       NOT NULL DEFAULT 0,
  materialModifier            smallint  NOT NULL DEFAULT 0,
  wasteFactor                 smallint  NOT NULL DEFAULT 100,
  chanceOfReverseEngineering  float     NOT NULL DEFAULT 0.0,
  maxProductionLimit          int       NOT NULL DEFAULT 1000,
  CONSTRAINT pk_invBlueprintTypes PRIMARY KEY CLUSTERED (blueprintTypeID)
)

CREATE TABLE dbo.invControlTowerResourcePurposes
(
  purpose      tinyint       NOT NULL,
  purposeText  varchar(100)  NOT NULL,
  CONSTRAINT pk_invControlTowerResourcePurposes PRIMARY KEY CLUSTERED (purpose)
)

CREATE TABLE dbo.invControlTowerResources
(
  controlTowerTypeID  smallint  NOT NULL,
  resourceTypeID      smallint  NOT NULL,
  purpose             tinyint   NOT NULL,
  quantity            int       NOT NULL,
  minSecurityLevel    float     NULL,
  factionID           int       NULL,
  CONSTRAINT pk_invControlTowerResources PRIMARY KEY CLUSTERED (controlTowerTypeID, resourceTypeID)
)

CREATE TABLE dbo.invContrabandTypes
(
  factionID         int       NOT NULL,
  typeID            smallint  NOT NULL,
  standingLoss      float     NOT NULL DEFAULT 0.0,
  confiscateMinSec  float     NOT NULL DEFAULT -1.0,
  fineByValue       float     NOT NULL DEFAULT 0.0,
  attackMinSec      float     NOT NULL DEFAULT -1.0,
  CONSTRAINT pk_invContrabandTypes PRIMARY KEY CLUSTERED (factionID, typeID)
)

CREATE TABLE dbo.eveGraphics
(
  graphicID                     smallint        NOT NULL,
  url3D                         varchar(100)    NOT NULL DEFAULT '',
  urlWeb                        varchar(100)    NOT NULL DEFAULT '',
  [description]                 varchar(1000)   NOT NULL DEFAULT '',
  published                     bit             NOT NULL DEFAULT 1,
  obsolete                      bit             NOT NULL DEFAULT 0,
  icon                          varchar(100)    NOT NULL DEFAULT '',
  urlSound                      varchar(100)    NOT NULL DEFAULT '',
  explosionID                   smallint        NULL,
  CONSTRAINT pk_eveGraphics PRIMARY KEY CLUSTERED (graphicID)
)

CREATE TABLE dbo.eveNames
(
  itemID                        int             NOT NULL,
  itemName                      nvarchar(100)   COLLATE Latin1_General_CI_AI NOT NULL,
  categoryID                    tinyint         NOT NULL,
  groupID                       tinyint         NOT NULL,
  typeID                        smallint        NOT NULL,
  CONSTRAINT pk_eveNames PRIMARY KEY CLUSTERED (itemID)
) 

CREATE TABLE dbo.dgmAttributeTypes
(
  attributeID                   int             NOT NULL,
  attributeName                 varchar(100)    COLLATE Latin1_General_CI_AI NOT NULL,
  attributeCategory             smallint        NOT NULL,
  [description]                 varchar(1000)   NOT NULL,
  maxAttributeID                int             NULL,
  attributeIdx                  int             NULL,
  graphicID                     smallint        NULL,
  chargeRechargeTimeID          int             NULL,
  defaultValue                  int             NOT NULL DEFAULT 0,
  published                     bit             NOT NULL DEFAULT 1,
  displayName                   varchar(100)    NOT NULL DEFAULT '',
  unitID                        tinyint         NULL,
  stackable                     bit             NOT NULL DEFAULT 1,
  highIsGood                    bit             NOT NULL DEFAULT 1,
  CONSTRAINT pk_dgmAttributeTypes PRIMARY KEY CLUSTERED (attributeID)
)

CREATE TABLE dbo.dgmTypeAttributes
(
  typeID                        smallint        NOT NULL,
  attributeID                   int             NOT NULL,
  valueInt                      int             NULL,
  valueFloat                    float           NULL,
  CONSTRAINT pk_dgmTypeAttributes PRIMARY KEY CLUSTERED (typeID, attributeID)
)

CREATE TABLE dbo.dgmEffects
(
  effectID                        int            NOT NULL,
  effectName                      varchar(400)   COLLATE Latin1_General_CI_AI NOT NULL,
  effectCategory                  smallint       NOT NULL,
  preExpression                   int            NULL,
  postExpression                  int            NULL,
  [description]                   varchar(1000)  NULL,
  guid                            varchar(60)    NULL,
  graphicID                       smallint       NULL,
  isOffensive                     bit            NOT NULL DEFAULT 0,
  isAssistance                    bit            NOT NULL DEFAULT 0,
  durationAttributeID             int            NULL,
  trackingSpeedAttributeID        int            NULL,
  dischargeAttributeID            int            NULL,
  rangeAttributeID                int            NULL,
  falloffAttributeID              int            NULL,
  disallowAutoRepeat              bit            NOT NULL DEFAULT 0,
  published                       bit            NOT NULL DEFAULT 1,
  displayName                     varchar(100)   NOT NULL DEFAULT '',
  isWarpSafe                      bit            NOT NULL DEFAULT 0,
  rangeChance                     bit            NOT NULL DEFAULT 0,
  electronicChance                bit            NOT NULL DEFAULT 0,
  propulsionChance                bit            NOT NULL DEFAULT 0,
  distribution                    tinyint        NULL,
  sfxName                         varchar(20)    NULL,
  npcUsageChanceAttributeID       int            NULL,
  npcActivationChanceAttributeID  int            NULL,
  fittingUsageChanceAttributeID   int            NULL,
  CONSTRAINT pk_dgmEffects PRIMARY KEY CLUSTERED (effectID)
)

CREATE TABLE dbo.dgmTypeEffects
(
  typeID                        smallint        NOT NULL,
  effectID                      int             NOT NULL,
  isDefault                     bit             NOT NULL DEFAULT 0,
  CONSTRAINT pk_dgmTypeEffects PRIMARY KEY CLUSTERED (typeID, effectID)
)

CREATE TABLE dbo.crpActivities
(
  activityID                    tinyint         NOT NULL,
  activityName                  nvarchar(100)   NOT NULL DEFAULT '',
  [description]                 nvarchar(1000)  NOT NULL DEFAULT '',
  CONSTRAINT pk_crpActivities PRIMARY KEY CLUSTERED (activityID)
)

CREATE TABLE dbo.crpNPCCorporations
(
  corporationID                 int             NOT NULL,
  mainActivityID                tinyint         NOT NULL,
  secondaryActivityID           tinyint         NULL,
  [size]                        char(1)         NOT NULL DEFAULT 'T',
  extent                        char(1)         NOT NULL DEFAULT 'L',
  solarSystemID                 int             NULL,
  investorID1                   int             NULL,
  investorShares1               tinyint         NOT NULL DEFAULT 0,
  investorID2                   int             NULL,
  investorShares2               tinyint         NOT NULL DEFAULT 0,
  investorID3                   int             NULL,
  investorShares3               tinyint         NOT NULL DEFAULT 0,
  investorID4                   int             NULL,
  investorShares4               tinyint         NOT NULL DEFAULT 0,
  friendID                      int             NULL,
  enemyID                       int             NULL,
  publicShares                  bigint          NOT NULL DEFAULT 0,
  initialPrice                  int             NOT NULL DEFAULT 0,
  minSecurity                   float           NOT NULL DEFAULT 0.0,
  scattered                     bit             NOT NULL DEFAULT 0,
  fringe                        tinyint         NOT NULL DEFAULT 0,
  corridor                      tinyint         NOT NULL DEFAULT 0,
  hub                           tinyint         NOT NULL DEFAULT 0,
  border                        tinyint         NOT NULL DEFAULT 0,
  factionID                     int             NOT NULL,
  sizeFactor                    float           NOT NULL,
  stationCount                  smallint        NOT NULL DEFAULT 0,
  stationSystemCount            smallint        NOT NULL DEFAULT 0,
  CONSTRAINT pk_crpNPCCorporations PRIMARY KEY CLUSTERED (corporationID)
)

CREATE TABLE dbo.crpNPCCorporationResearchFields
(
  skillID                       smallint        NOT NULL,
  corporationID                 int             NOT NULL,
  supplierTypes                 tinyint         NOT NULL,
  CONSTRAINT pk_crpNPCCorporationResearchFields PRIMARY KEY CLUSTERED (skillID, corporationID)
)

CREATE TABLE dbo.crpNPCCorporationTrades
(
  corporationID                 int             NOT NULL,
  typeID						int             NOT NULL,
  supplyDemand                  float           NOT NULL,
  CONSTRAINT pk_crpNPCCorporationTrades PRIMARY KEY CLUSTERED (corporationID, typeID)
)

CREATE TABLE dbo.crpNPCDivisions
(
  divisionID                    tinyint         NOT NULL,
  divisionName                  nvarchar(100)   NOT NULL,
  [description]                 nvarchar(1000)  NOT NULL DEFAULT '',
  leaderType                    nvarchar(100)   NOT NULL DEFAULT '',
  CONSTRAINT pk_crpNPCDivisions PRIMARY KEY CLUSTERED (divisionID)
)

CREATE TABLE dbo.crpNPCCorporationDivisions
(
  corporationID                 int             NOT NULL,
  divisionID                    tinyint         NOT NULL,
  divisionNumber                tinyint         NOT NULL,
  [size]                        tinyint         NULL,
  leaderID                      int             NULL,
  CONSTRAINT pk_crpNPCCorporationDivisions PRIMARY KEY CLUSTERED (corporationID, divisionID)
)

CREATE TABLE dbo.chrAttributes
(
  attributeID       tinyint         NOT NULL,
  attributeName     varchar(100)    NOT NULL DEFAULT '',
  [description]     varchar(1000)   NOT NULL DEFAULT '',
  graphicID         smallint        NULL,
  shortDescription  nvarchar(500)   NOT NULL DEFAULT '',
  notes             nvarchar(500)   NOT NULL DEFAULT '',
  CONSTRAINT pk_chrAttributes PRIMARY KEY CLUSTERED (attributeID)
)

CREATE TABLE dbo.chrRaces
(
  raceID             tinyint         NOT NULL,
  raceName           nvarchar(100)   NOT NULL DEFAULT '',
  [description]      nvarchar(1000)  NOT NULL DEFAULT '',
  skillTypeID1       smallint        NULL,
  typeID             smallint        NULL,
  typeQuantity       tinyint         NULL,
  graphicID          smallint        NULL,
  shortDescription   nvarchar(500)   NOT NULL DEFAULT '',
  CONSTRAINT pk_chrRaces PRIMARY KEY CLUSTERED (raceID)
)

CREATE TABLE dbo.chrBloodlines
(
  bloodlineID             tinyint         NOT NULL,
  bloodlineName           nvarchar(100)   NOT NULL DEFAULT '',
  raceID                  tinyint         NOT NULL DEFAULT 0,
  [description]           nvarchar(1000)  NOT NULL DEFAULT '',
  maleDescription         nvarchar(1000)  NOT NULL DEFAULT '',
  femaleDescription       nvarchar(1000)  NOT NULL DEFAULT '',
  shipTypeID              smallint        NOT NULL,
  corporationID           int             NOT NULL,
  perception              tinyint         NOT NULL DEFAULT 0,
  willpower               tinyint         NOT NULL DEFAULT 0,
  charisma                tinyint         NOT NULL DEFAULT 0,
  memory                  tinyint         NOT NULL DEFAULT 0,
  intelligence            tinyint         NOT NULL DEFAULT 0,
  graphicID               smallint        NULL,
  shortDescription        nvarchar(500)   NOT NULL DEFAULT '',
  shortMaleDescription    nvarchar(500)   NOT NULL DEFAULT '',
  shortFemaleDescription  nvarchar(500)   NOT NULL DEFAULT '',
  CONSTRAINT pk_chrBloodlines PRIMARY KEY CLUSTERED (bloodlineID)
)

CREATE TABLE dbo.chrAncestries
(
  ancestryID        tinyint         NOT NULL,
  ancestryName      nvarchar(100)   NOT NULL DEFAULT '',
  bloodlineID       tinyint         NOT NULL DEFAULT 0,
  [description]     nvarchar(1000)  NOT NULL DEFAULT '',
  perception        tinyint         NOT NULL DEFAULT 0,
  willpower         tinyint         NOT NULL DEFAULT 0,
  charisma          tinyint         NOT NULL DEFAULT 0,
  memory            tinyint         NOT NULL DEFAULT 0,
  intelligence      tinyint         NOT NULL DEFAULT 0,
  skillTypeID1      smallint        NULL,
  skillTypeID2      smallint        NULL,
  typeID            smallint        NULL,
  typeQuantity      tinyint         NULL,
  typeID2           smallint        NULL,
  typeQuantity2     tinyint         NULL,
  graphicID         smallint        NULL,
  shortDescription  nvarchar(500)   NOT NULL DEFAULT '',
  CONSTRAINT pk_chrAncestries PRIMARY KEY CLUSTERED (ancestryID)
)

CREATE TABLE dbo.chrCareers
(
  raceID            tinyint         NOT NULL,
  careerID          tinyint         NOT NULL,
  careerName        nvarchar(100)   NOT NULL DEFAULT '',
  [description]     nvarchar(2000)  NOT NULL DEFAULT '',
  shortDescription  nvarchar(500)   NOT NULL DEFAULT '',
  graphicID         smallint        NULL,
  schoolID          tinyint         NULL,
  CONSTRAINT pk_chrCareers PRIMARY KEY CLUSTERED (careerID)
)

CREATE TABLE dbo.chrCareerSpecialities
(
  careerID          tinyint         NOT NULL,
  specialityID      tinyint         NOT NULL,
  specialityName    nvarchar(100)   NOT NULL DEFAULT '',
  [description]     nvarchar(2000)  NOT NULL DEFAULT '',
  shortDescription  nvarchar(500)   NOT NULL DEFAULT '',
  graphicID         smallint        NULL,
  departmentID      tinyint         NULL,
  CONSTRAINT pk_chrCareerSpecialities PRIMARY KEY CLUSTERED (specialityID)
)

CREATE TABLE dbo.chrRaceSkills
(
  raceID       tinyint   NOT NULL,
  skillTypeID  smallint  NOT NULL,
  levels       tinyint   NOT NULL,
  CONSTRAINT pk_chrRaceSkills PRIMARY KEY CLUSTERED (raceID, skillTypeID)
)

CREATE TABLE dbo.chrCareerSkills
(
  careerID     tinyint   NOT NULL,
  skillTypeID  smallint  NOT NULL,
  levels       tinyint   NOT NULL,
  CONSTRAINT pk_chrCareerSkills PRIMARY KEY CLUSTERED (careerID, skillTypeID)
)

CREATE TABLE dbo.chrCareerSpecialitySkills
(
  specialityID  tinyint   NOT NULL,
  skillTypeID   smallint  NOT NULL,
  levels        tinyint   NOT NULL,
  CONSTRAINT pk_chrCareerSpecialitySkills PRIMARY KEY CLUSTERED (specialityID, skillTypeID)
)

CREATE TABLE dbo.chrSchools
(
  raceID             tinyint         NOT NULL,
  schoolID           tinyint         NOT NULL,
  schoolName         nvarchar(100)   NOT NULL DEFAULT '',
  [description]      nvarchar(1000)  NOT NULL DEFAULT '',
  graphicID          smallint        NULL,
  corporationID      int             NULL,
  agentID            int             NULL,
  newAgentID         int             NULL,
  tutorialContentID  int             NULL,
  careerID           tinyint         NULL,
  CONSTRAINT pk_chrSchools PRIMARY KEY CLUSTERED (schoolID)
)

CREATE TABLE dbo.agtAgents
(
  agentID               int             NOT NULL,
  divisionID            int             NULL,
  corporationID         int             NULL,
  stationID             int             NULL,
  [level]               tinyint         NULL,
  quality               smallint        NULL,
  agentTypeID           tinyint         NOT NULL,
  CONSTRAINT pk_agtAgents PRIMARY KEY CLUSTERED (agentID)
)

CREATE TABLE dbo.agtConfig
(
  agentID               int             NOT NULL,
  k                     varchar(50)     NOT NULL,
  v                     varchar(4000)   NOT NULL,
  CONSTRAINT pk_agtConfig PRIMARY KEY CLUSTERED (agentID, k)
)

CREATE TABLE dbo.agtAgentTypes
(
  agentTypeID           tinyint         NOT NULL,
  agentType             varchar(50)     NOT NULL,
  CONSTRAINT pk_agtAgentTypes PRIMARY KEY CLUSTERED (agentTypeID)
)

CREATE TABLE dbo.agtResearchAgents
(
	agentID				int				NOT NULL,
	typeID				smallint		NOT NULL,
    CONSTRAINT agtResearchAgents_PK PRIMARY KEY CLUSTERED (agentID, typeID)
 )

CREATE TABLE dbo.chrFactions
(
  factionID  		int			NOT NULL,
  factionName  		nvarchar(100)	NOT NULL,
  description  		nvarchar(1000)	NOT NULL,
  raceIDs  			int				NOT NULL,
  solarSystemID  		int			NULL,
  corporationID  		int			NULL,
  sizeFactor  		float			NOT NULL,
  stationCount  		int			NOT NULL,
  stationSystemCount  	int			NOT NULL,
  militiaCorporationID	int			NULL,
  CONSTRAINT pk_chrFactions PRIMARY KEY CLUSTERED (factionID)
)

CREATE TABLE dbo.chrSchoolAgents
(
  schoolID  		int			NOT NULL,
  agentindex  		int			NOT NULL,
  agentID	  		int			NOT NULL,
  CONSTRAINT pk_chrSchoolAgents PRIMARY KEY CLUSTERED (agentID)
)

CREATE TABLE dbo.eveUnits
(
  unitID	  		int			NOT NULL,
  unitName  		nvarchar(100)	NOT NULL,
  displayName		nvarchar(20)	NOT NULL,
  description		nvarchar(1000)	NOT NULL,
  CONSTRAINT pk_eveUnits PRIMARY KEY CLUSTERED (unitID)
)

CREATE TABLE dbo.invFlags
(
  flagID	  		int			NOT NULL,
  flagName  		nvarchar(100)	NOT NULL,
  flagText			nvarchar(100)	NOT NULL,
  flagType			nvarchar(100)	NULL,
  orderID			int			NOT NULL,
  CONSTRAINT pk_invFlags PRIMARY KEY CLUSTERED (flagID)
)

CREATE TABLE dbo.invMetaGroups
(
  metaGroupID  		int			NOT NULL,
  metaGroupName  	nvarchar(100)	NOT NULL,
  description		nvarchar(1000)	NULL,
  graphicID			int			NULL,
  CONSTRAINT pk_invMetaGroups PRIMARY KEY CLUSTERED (metaGroupID)
)

CREATE TABLE dbo.invMetaTypes
(
  typeID	  		int			NOT NULL,
  parentTypeID  	int			NOT NULL,
  metaGroupID		int			NOT NULL,
  CONSTRAINT pk_invMetaTypes PRIMARY KEY CLUSTERED (typeID)
)

CREATE TABLE dbo.invTypeReactions
(
  reactionTypeID  	int			NOT NULL,
  input  			bit			NOT NULL,
  typeID			int			NOT NULL,
  quantity			int			NOT NULL,
 )

CREATE TABLE dbo.ramTypeRequirements
(
  typeID  			int			NOT NULL,
  activity  		int			NOT NULL,
  requiredTypeID	int			NOT NULL,
  quantity			int			NOT NULL,
  damagePerJob		float			NOT NULL,
 )

