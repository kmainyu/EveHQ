-- Rubicon 1.0.1 item DB changes to be applied to Rubicon 1.0 SDE
--
-- To get the invention changes into the cache files you need a fresh 
-- Rubicon 1.0 SDE not modified by CacheCreator, apply the queries in 
-- this file and then run "Check SQL Database" in CacheCreator.
--
-- This file can be removed when an SDE newer than Rubicon 1.0 is released.

-- Invention requirements added to tier 2 BC BPs
INSERT INTO ramTypeRequirements VALUES (24699, 8, 20424, 16, 1, 0);
INSERT INTO ramTypeRequirements VALUES (24699, 8, 25853, 1, 1, 0);
INSERT INTO ramTypeRequirements VALUES (24699, 8, 25887, 16, 1, 0);
INSERT INTO ramTypeRequirements VALUES (24701, 8, 20410, 16, 1, 0);
INSERT INTO ramTypeRequirements VALUES (24701, 8, 20424, 16, 1, 0);
INSERT INTO ramTypeRequirements VALUES (24701, 8, 25855, 1, 1, 0);
INSERT INTO ramTypeRequirements VALUES (24703, 8, 20172, 16, 1, 0);
INSERT INTO ramTypeRequirements VALUES (24703, 8, 20424, 16, 1, 0);
INSERT INTO ramTypeRequirements VALUES (24703, 8, 25857, 1, 1, 0);
INSERT INTO ramTypeRequirements VALUES (24697, 8, 20421, 16, 1, 0);
INSERT INTO ramTypeRequirements VALUES (24697, 8, 20424, 16, 1, 0);
INSERT INTO ramTypeRequirements VALUES (24697, 8, 25851, 1, 1, 0);

-- Changed size of Bastion Module I
UPDATE invTypes SET volume = 200 WHERE typeID = 33400;

-- CovOps cloak allowed on Stratios Emergency Responder
INSERT INTO dgmTypeAttributes VALUES (11578, 1305, NULL, 33553);