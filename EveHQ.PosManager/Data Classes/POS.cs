using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace EveHQ.PosManager
{
    [Serializable]
    public class POS
    {
        public Tower PosTower;
        public ArrayList Modules;
        public DateTime Fuel_TS, Stront_TS, API_TS;
        public ArrayList Extra;

        public string Name;
        public int itemID, locID;
        public bool Monitored;

        public POS()
        {
            PosTower = new Tower();
            Name = "";
            itemID = 0;
            locID = 0;
            Modules = new ArrayList();
            Monitored = false;
            Fuel_TS = DateTime.Now;
            Stront_TS = DateTime.Now;
            API_TS = DateTime.Now;
            Extra = new ArrayList();
        }

        public POS(string nm)
        {
            PosTower = new Tower();
            Name = nm;
            itemID = 0;
            locID = 0;
            Modules = new ArrayList();
            Monitored = false;
            Fuel_TS = DateTime.Now;
            Stront_TS = DateTime.Now;
            API_TS = DateTime.Now;
            Extra = new ArrayList();
        }

        public POS(POS p)
        {
            PosTower = new Tower(p.PosTower);
            Name = p.Name;
            itemID = p.itemID;
            locID = p.locID;
            Modules = new ArrayList(p.Modules);
            Monitored = p.Monitored;
            Fuel_TS = p.Fuel_TS;
            Stront_TS = p.Stront_TS;
            API_TS = p.API_TS;
            Extra = new ArrayList(p.Extra);
        }

        public POS(string nm, POS p)
        {
            PosTower = new Tower(p.PosTower);
            Name = nm;
            itemID = p.itemID;
            locID = p.locID;
            Modules = new ArrayList(p.Modules);
            Monitored = p.Monitored;
            Fuel_TS = p.Fuel_TS;
            Stront_TS = p.Stront_TS;
            API_TS = p.API_TS;
            Extra = new ArrayList(p.Extra);
        }

        public void ClearAllPOSData()
        {
            PosTower = new Tower();
            Name = "";
            itemID = 0;
            locID = 0;
            Modules.Clear();
            Monitored = false;
            Fuel_TS = DateTime.Now;
            Stront_TS = DateTime.Now;
            API_TS = DateTime.Now;
            Extra.Clear();
        }

        public POS CopyPOSData()
        {
            POS p = new POS();

            p.PosTower = new Tower(PosTower);
            p.Name = Name;
            p.itemID = itemID;
            p.locID = locID;
            p.Modules = new ArrayList(Modules);
            p.Monitored = Monitored;
            p.Fuel_TS = Fuel_TS;
            p.Stront_TS = Stront_TS;
            p.API_TS = API_TS;
            p.Extra = new ArrayList(Extra);

            return p;
        }

        public void RemoveModuleFromPOS(int rowIndex)
        {
            Modules.RemoveAt(rowIndex);
        }

        public void RemoveTowerFromPOS()
        {
            PosTower = new Tower();
        }

        public decimal GetVolumeForFuel(decimal qty, decimal vol, decimal qtyVol)
        {
            decimal retVal;

            if (qtyVol > 0)
                retVal = ((qty / qtyVol) * vol);
            else
                retVal = 0;

            return retVal;
        }

        public decimal GetFuelRunTime(decimal qty, decimal pdQty)
        {
            if (pdQty > 0)
                return (qty / pdQty);
            else
                return 0;
        }

        public decimal GetModifierTime(decimal used, decimal cap)
        {
            if (cap > 0)
                return (used / cap);
            else
                return 1;
        }
        
        public void CalculatePOSFuelRunTime(API_List APIL)
        {
            string status;
            DateTime F_TimeStamp, S_TimeStamp, C_TimeStamp, A_TimeStamp;
            DateTime A_In_TimeStamp = DateTime.Now;
            TimeSpan D_TimeStamp;
            API_Data apid = new API_Data();
            decimal enr_uran = 0, oxygen = 0, mech_part = 0, robotic = 0;
            decimal coolant = 0, hvy_water = 0, liq_ozone = 0, stront = 0;
            decimal charter = 0, h2_iso = 0, n2_iso = 0, he_iso = 0, o2_iso = 0;
            decimal m_fb_use = 0, m_sb_use = 0;
            decimal m_enr_uran = 0, m_oxygen = 0, m_mech_part = 0, m_robotic = 0;
            decimal m_coolant = 0, m_hvy_water = 0, m_liq_ozone = 0, m_stront = 0;
            decimal m_charter = 0, m_h2_iso = 0, m_n2_iso = 0, m_he_iso = 0, m_o2_iso = 0;
            decimal f_run_time, s_run_time, m_run_time;
            decimal cpu, cpu_used, power, power_used;
            decimal cpu_mod, power_mod;
            int F_HourDiff = 0, S_HourDiff = 0, typeID = 0;
            int F_DayDiff = 0, F_MinDiff = 0, S_DayDiff = 0, S_MinDiff = 0;

            C_TimeStamp = DateTime.Now;

            // This function will calculate the fuel run times for the current POS.
            // Get Fuel calc start timestamp
            F_TimeStamp = Fuel_TS;
            S_TimeStamp = Stront_TS;
            A_TimeStamp = API_TS;
            status = PosTower.State;

            // Get Base POS Tower Fuel Numbers - ie: Amount per Period (1 hour typically)
            enr_uran = PosTower.Fuel.EnrUran.PeriodQty;
            oxygen = PosTower.Fuel.Oxygen.PeriodQty;
            mech_part = PosTower.Fuel.MechPart.PeriodQty;
            coolant = PosTower.Fuel.Coolant.PeriodQty;
            robotic = PosTower.Fuel.Robotics.PeriodQty;
            hvy_water = PosTower.Fuel.HvyWater.PeriodQty;
            liq_ozone = PosTower.Fuel.LiqOzone.PeriodQty;
            charter = PosTower.Fuel.Charters.PeriodQty;
            n2_iso = PosTower.Fuel.N2Iso.PeriodQty;
            he_iso = PosTower.Fuel.HeIso.PeriodQty;
            h2_iso = PosTower.Fuel.H2Iso.PeriodQty;
            o2_iso = PosTower.Fuel.O2Iso.PeriodQty;
            stront = PosTower.Fuel.Strontium.PeriodQty;

            // Get monitored POS Fuel Values - ie: Not the design time values
            if (itemID != 0)
            {
                // LINKED POS

                apid = APIL.GetAPIDataMemberForTowerID(itemID);
                A_In_TimeStamp = Convert.ToDateTime(apid.cacheUntil);
                // Now, while this is accurate, need to remove 24 hours from the time
                // to make it work correctly
                A_In_TimeStamp = A_In_TimeStamp.Subtract(TimeSpan.FromHours(24));
                A_In_TimeStamp = TimeZone.CurrentTimeZone.ToLocalTime(A_In_TimeStamp);

                if (A_TimeStamp.Equals(A_In_TimeStamp))
                {
                    // POS is linked, API is Current, use internal Data
                    m_enr_uran = PosTower.Fuel.EnrUran.Qty;
                    m_oxygen = PosTower.Fuel.Oxygen.Qty;
                    m_mech_part = PosTower.Fuel.MechPart.Qty;
                    m_coolant = PosTower.Fuel.Coolant.Qty;
                    m_robotic = PosTower.Fuel.Robotics.Qty;
                    m_hvy_water = PosTower.Fuel.HvyWater.Qty;
                    m_liq_ozone = PosTower.Fuel.LiqOzone.Qty;
                    m_charter = PosTower.Fuel.Charters.Qty;
                    m_n2_iso = PosTower.Fuel.N2Iso.Qty;
                    m_he_iso = PosTower.Fuel.HeIso.Qty;
                    m_h2_iso = PosTower.Fuel.H2Iso.Qty;
                    m_o2_iso = PosTower.Fuel.O2Iso.Qty;
                    m_stront = PosTower.Fuel.Strontium.Qty;
                }
                else
                {
                    F_TimeStamp = A_In_TimeStamp;
                    S_TimeStamp = A_In_TimeStamp;

                    m_enr_uran = apid.EnrUr;
                    m_oxygen = apid.Oxygn;
                    m_mech_part = apid.MechP;
                    m_coolant = apid.Coolt;
                    m_robotic = apid.Robot;
                    m_hvy_water = apid.HvyWt;
                    m_liq_ozone = apid.LiqOz;
                    m_charter = apid.Charters;
                    m_n2_iso = apid.N2Iso;
                    m_he_iso = apid.HeIso;
                    m_h2_iso = apid.H2Iso;
                    m_o2_iso = apid.O2Iso;
                    m_stront = (int)apid.Stront;
                    Fuel_TS = F_TimeStamp;
                    API_TS = A_In_TimeStamp;
                }
            }
            else
            {
                // UNLINKED POS
                // POS is NOT linked
                m_enr_uran = PosTower.Fuel.EnrUran.Qty;
                m_oxygen = PosTower.Fuel.Oxygen.Qty;
                m_mech_part = PosTower.Fuel.MechPart.Qty;
                m_coolant = PosTower.Fuel.Coolant.Qty;
                m_robotic = PosTower.Fuel.Robotics.Qty;
                m_hvy_water = PosTower.Fuel.HvyWater.Qty;
                m_liq_ozone = PosTower.Fuel.LiqOzone.Qty;
                m_charter = PosTower.Fuel.Charters.Qty;
                m_n2_iso = PosTower.Fuel.N2Iso.Qty;
                m_he_iso = PosTower.Fuel.HeIso.Qty;
                m_h2_iso = PosTower.Fuel.H2Iso.Qty;
                m_o2_iso = PosTower.Fuel.O2Iso.Qty;
                m_stront = PosTower.Fuel.Strontium.Qty;
            }
            typeID = PosTower.typeID;

            // Determine the difference in time between the Timestamp values, and current time
            D_TimeStamp = C_TimeStamp.Subtract(F_TimeStamp);
            F_HourDiff = D_TimeStamp.Hours;
            F_DayDiff = D_TimeStamp.Days;
            F_MinDiff = D_TimeStamp.Minutes;
            F_HourDiff += F_DayDiff * 24;
            D_TimeStamp = C_TimeStamp.Subtract(S_TimeStamp);
            S_HourDiff = D_TimeStamp.Hours;
            S_DayDiff = D_TimeStamp.Days;
            S_MinDiff = D_TimeStamp.Minutes;
            S_HourDiff += S_DayDiff * 24;

            if (status == "Offline")
            {
                F_HourDiff = 0;
                S_HourDiff = 0;
            }
            else if (status == "Reinforced")
            {
                if (S_HourDiff > 0)
                {
                    // At least 1 hour has expired, so we can reset the timestamp
                    Stront_TS = C_TimeStamp.Subtract(TimeSpan.FromMinutes(S_MinDiff));
                }
                // Tower is in Reinforced mode, Compute Strontium Usage and Run Time
                m_stront -= stront * S_HourDiff;
            }

            if (F_HourDiff > 0)
            {
                // At least 1 hour has expired, so we can reset the timestamp
                Fuel_TS = C_TimeStamp.Subtract(TimeSpan.FromMinutes(F_MinDiff));
            }

            // Determine actual CPU and POWER Fuel qty #s based upon CPU and Power usage of design
            cpu = PosTower.CPU;
            cpu_used = PosTower.CPU_Used;

            power = PosTower.Power;
            power_used = PosTower.Power_Used;

            cpu_mod = GetModifierTime(cpu_used, cpu);
            power_mod = GetModifierTime(power_used, power);

            hvy_water = Math.Ceiling(hvy_water * cpu_mod);
            liq_ozone = Math.Ceiling(liq_ozone * power_mod);

            // Compute Fuel Usage
            m_enr_uran -= enr_uran * F_HourDiff;
            m_oxygen -= oxygen * F_HourDiff;
            m_mech_part -= mech_part * F_HourDiff;
            m_coolant -= coolant * F_HourDiff;
            m_robotic -= robotic * F_HourDiff;
            m_hvy_water -= hvy_water * F_HourDiff;
            m_liq_ozone -= liq_ozone * F_HourDiff;
            m_charter -= charter * F_HourDiff;
            m_n2_iso -= n2_iso * F_HourDiff;
            m_he_iso -= he_iso * F_HourDiff;
            m_h2_iso -= h2_iso * F_HourDiff;
            m_o2_iso -= o2_iso * F_HourDiff;

            // Negatives are mathematically possible here due to Online/Offline changes
            // in state for monitored, but they are impossible in the game. So just
            // Zero out any negative value.
            if (m_enr_uran < 0)
                m_enr_uran = 0;
            if (m_oxygen < 0)
                m_oxygen = 0;
            if (m_mech_part < 0)
                m_mech_part = 0;
            if (m_coolant < 0)
                m_coolant = 0;
            if (m_robotic < 0)
                m_robotic = 0;
            if (m_hvy_water < 0)
                m_hvy_water = 0;
            if (m_liq_ozone < 0)
                m_liq_ozone = 0;
            if (m_charter < 0)
                m_charter = 0;
            if (m_n2_iso < 0)
                m_n2_iso = 0;
            if (m_h2_iso < 0)
                m_h2_iso = 0;
            if (m_o2_iso < 0)
                m_o2_iso = 0;
            if (m_he_iso < 0)
                m_he_iso = 0;
            if (m_stront < 0)
                m_stront = 0;

            // Calculate Fuel Bay Volume (and Stront) based on Individual Fuel Volumes.
            m_fb_use = 0;
            m_fb_use += GetVolumeForFuel(m_enr_uran, PosTower.Fuel.EnrUran.BaseVol, PosTower.Fuel.EnrUran.QtyVol);
            m_fb_use += GetVolumeForFuel(m_oxygen, PosTower.Fuel.Oxygen.BaseVol, PosTower.Fuel.Oxygen.QtyVol);
            m_fb_use += GetVolumeForFuel(m_mech_part, PosTower.Fuel.MechPart.BaseVol, PosTower.Fuel.MechPart.QtyVol);
            m_fb_use += GetVolumeForFuel(m_coolant, PosTower.Fuel.Coolant.BaseVol, PosTower.Fuel.Coolant.QtyVol);
            m_fb_use += GetVolumeForFuel(m_robotic, PosTower.Fuel.Robotics.BaseVol, PosTower.Fuel.Robotics.QtyVol);
            m_fb_use += GetVolumeForFuel(m_hvy_water, PosTower.Fuel.HvyWater.BaseVol, PosTower.Fuel.HvyWater.QtyVol);
            m_fb_use += GetVolumeForFuel(m_liq_ozone, PosTower.Fuel.LiqOzone.BaseVol, PosTower.Fuel.LiqOzone.QtyVol);
            m_fb_use += GetVolumeForFuel(m_charter, PosTower.Fuel.Charters.BaseVol, PosTower.Fuel.Charters.QtyVol);
            m_fb_use += GetVolumeForFuel(m_n2_iso, PosTower.Fuel.N2Iso.BaseVol, PosTower.Fuel.N2Iso.QtyVol);
            m_fb_use += GetVolumeForFuel(m_h2_iso, PosTower.Fuel.H2Iso.BaseVol, PosTower.Fuel.H2Iso.QtyVol);
            m_fb_use += GetVolumeForFuel(m_o2_iso, PosTower.Fuel.O2Iso.BaseVol, PosTower.Fuel.O2Iso.QtyVol);
            m_fb_use += GetVolumeForFuel(m_he_iso, PosTower.Fuel.HeIso.BaseVol, PosTower.Fuel.HeIso.QtyVol);
            m_sb_use = GetVolumeForFuel(m_stront, PosTower.Fuel.Strontium.BaseVol, PosTower.Fuel.Strontium.QtyVol);

            // Store new POS Fuel Data Values
            PosTower.Fuel.EnrUran.Qty = m_enr_uran;
            PosTower.Fuel.Oxygen.Qty = m_oxygen;
            PosTower.Fuel.MechPart.Qty = m_mech_part;
            PosTower.Fuel.Coolant.Qty = m_coolant;
            PosTower.Fuel.Robotics.Qty = m_robotic;
            PosTower.Fuel.HvyWater.Qty = m_hvy_water;
            PosTower.Fuel.LiqOzone.Qty = m_liq_ozone;
            PosTower.Fuel.Charters.Qty = m_charter;
            PosTower.Fuel.N2Iso.Qty = m_n2_iso;
            PosTower.Fuel.HeIso.Qty = m_he_iso;
            PosTower.Fuel.H2Iso.Qty = m_h2_iso;
            PosTower.Fuel.O2Iso.Qty = m_o2_iso;
            PosTower.Fuel.Strontium.Qty = m_stront;

            PosTower.Fuel.FuelUsed = m_fb_use;
            PosTower.Fuel.StrontUsed = m_sb_use;

            // Calculate Stront and Fuel Usage Run Times
            s_run_time = GetFuelRunTime(m_stront, stront);
            PosTower.Fuel.Strontium.RunTime = s_run_time;
            PosTower.S_RunTime = s_run_time;

            m_run_time = 9999999999999999;

            f_run_time = GetFuelRunTime(m_enr_uran, enr_uran);
            PosTower.Fuel.EnrUran.RunTime = f_run_time;
            if (f_run_time < m_run_time)
            {
                m_run_time = f_run_time;
                PosTower.F_RunTime = m_run_time;
                PosTower.Low_Fuel = "Enriched Uranium";
            }

            f_run_time = GetFuelRunTime(m_oxygen, oxygen);
            PosTower.Fuel.Oxygen.RunTime = f_run_time;
            if (f_run_time < m_run_time)
            {
                m_run_time = f_run_time;
                PosTower.F_RunTime = m_run_time;
                PosTower.Low_Fuel = "Oxygen";
            }

            f_run_time = GetFuelRunTime(m_mech_part, mech_part);
            PosTower.Fuel.MechPart.RunTime = f_run_time;
            if (f_run_time < m_run_time)
            {
                m_run_time = f_run_time;
                PosTower.F_RunTime = m_run_time;
                PosTower.Low_Fuel = "Mechanical Parts";
            }

            f_run_time = GetFuelRunTime(m_coolant, coolant);
            PosTower.Fuel.Coolant.RunTime = f_run_time;
            if (f_run_time < m_run_time)
            {
                m_run_time = f_run_time;
                PosTower.F_RunTime = m_run_time;
                PosTower.Low_Fuel = "Coolant";
            }

            f_run_time = GetFuelRunTime(m_robotic, robotic);
            PosTower.Fuel.Robotics.RunTime = f_run_time;
            if (f_run_time < m_run_time)
            {
                m_run_time = f_run_time;
                PosTower.F_RunTime = m_run_time;
                PosTower.Low_Fuel = "Robotics";
            }

            f_run_time = GetFuelRunTime(m_hvy_water, hvy_water);
            PosTower.Fuel.HvyWater.RunTime = f_run_time;
            if ((f_run_time < m_run_time) && (cpu_used > 0))
            {
                m_run_time = f_run_time;
                PosTower.F_RunTime = m_run_time;
                PosTower.Low_Fuel = "Heavy Water";
            }

            f_run_time = GetFuelRunTime(m_liq_ozone, liq_ozone);
            PosTower.Fuel.LiqOzone.RunTime = f_run_time;
            if ((f_run_time < m_run_time) && (power_used > 0))
            {
                m_run_time = f_run_time;
                PosTower.F_RunTime = m_run_time;
                PosTower.Low_Fuel = "Liquid Ozone";
            }

            f_run_time = GetFuelRunTime(m_charter, charter);
            PosTower.Fuel.Charters.RunTime = f_run_time;
            if (f_run_time < m_run_time)
            {
                m_run_time = f_run_time;
                PosTower.F_RunTime = m_run_time;
                PosTower.Low_Fuel = "Charters";
            }

            if (m_n2_iso > 0)
            {
                f_run_time = GetFuelRunTime(m_n2_iso, n2_iso);
                PosTower.Fuel.N2Iso.RunTime = f_run_time;
                if (f_run_time < m_run_time)
                {
                    m_run_time = f_run_time;
                    PosTower.F_RunTime = m_run_time;
                    PosTower.Low_Fuel = "Nitrogen Isotopes";
                }
            }
            else if (m_h2_iso > 0)
            {
                f_run_time = GetFuelRunTime(m_h2_iso, h2_iso);
                PosTower.Fuel.H2Iso.RunTime = f_run_time;
                if (f_run_time < m_run_time)
                {
                    m_run_time = f_run_time;
                    PosTower.F_RunTime = m_run_time;
                    PosTower.Low_Fuel = "Hydrogen Isotopes";
                }
            }
            else if (m_o2_iso > 0)
            {
                f_run_time = GetFuelRunTime(m_o2_iso, o2_iso);
                if (f_run_time < m_run_time)
                {
                    m_run_time = f_run_time;
                    PosTower.F_RunTime = m_run_time;
                    PosTower.Low_Fuel = "Oxygen Isotopes";
                }
            }
            else if (m_he_iso > 0)
            {
                f_run_time = GetFuelRunTime(m_he_iso, he_iso);
                if (f_run_time < m_run_time)
                {
                    m_run_time = f_run_time;
                    PosTower.F_RunTime = m_run_time;
                    PosTower.Low_Fuel = "Helium Isotopes";
                }
            }
            else
            {
                PosTower.Fuel.N2Iso.RunTime = 0;
                PosTower.Fuel.H2Iso.RunTime = 0;
                PosTower.Fuel.O2Iso.RunTime = 0;
                PosTower.Fuel.HeIso.RunTime = 0;
            }
        }

        public decimal ComputeMaxPosRunTimeForLoad()
        {
            decimal enr_uran = 0, oxygen = 0, mech_part = 0, robotic = 0;
            decimal coolant = 0, hvy_water = 0, liq_ozone = 0;
            decimal charter = 0, h2_iso = 0, n2_iso = 0, he_iso = 0, o2_iso = 0;
            decimal fuel_use = 0, fuel_cap = 0;
            decimal cpu, cpu_used, power, power_used;
            decimal cpu_mod, power_mod;
            decimal period = 0;

            fuel_cap = PosTower.D_Fuel.FuelCap;

            while (fuel_use < fuel_cap)
            {
                period++;

                // Get Base POS Tower Fuel Numbers - ie: Amount per Period (1 hour typically)
                enr_uran = PosTower.D_Fuel.EnrUran.PeriodQty;
                oxygen = PosTower.D_Fuel.Oxygen.PeriodQty;
                mech_part = PosTower.D_Fuel.MechPart.PeriodQty;
                coolant = PosTower.D_Fuel.Coolant.PeriodQty;
                robotic = PosTower.D_Fuel.Robotics.PeriodQty;
                hvy_water = PosTower.D_Fuel.HvyWater.PeriodQty;
                liq_ozone = PosTower.D_Fuel.LiqOzone.PeriodQty;
                charter = PosTower.D_Fuel.Charters.PeriodQty;
                n2_iso = PosTower.D_Fuel.N2Iso.PeriodQty;
                he_iso = PosTower.D_Fuel.HeIso.PeriodQty;
                h2_iso = PosTower.D_Fuel.H2Iso.PeriodQty;
                o2_iso = PosTower.D_Fuel.O2Iso.PeriodQty;

                // Modify POS Fuel Numbers on amount (multiplicative)
                enr_uran = enr_uran * period;
                oxygen = oxygen * period;
                mech_part = mech_part * period;
                coolant = coolant * period;
                robotic = robotic * period;
                charter = charter * period;
                n2_iso = n2_iso * period;
                he_iso = he_iso * period;
                h2_iso = h2_iso * period;
                o2_iso = o2_iso * period;

                // Determine actual CPU and POWER Fuel qty #s based upon CPU and Power usage of design
                cpu = PosTower.CPU;
                cpu_used = PosTower.CPU_Used;

                power = PosTower.Power;
                power_used = PosTower.Power_Used;

                cpu_mod = GetModifierTime(cpu_used, cpu);
                power_mod = GetModifierTime(power_used, power);

                hvy_water = Math.Ceiling(hvy_water * cpu_mod);
                liq_ozone = Math.Ceiling(liq_ozone * power_mod);

                hvy_water = hvy_water * period;
                liq_ozone = liq_ozone * period;

                // Calculate Fuel Bay Volume (and Stront) based on Individual Fuel Volumes.
                fuel_use = 0;
                fuel_use += GetVolumeForFuel(enr_uran, PosTower.D_Fuel.EnrUran.BaseVol, PosTower.D_Fuel.EnrUran.QtyVol);
                fuel_use += GetVolumeForFuel(oxygen, PosTower.D_Fuel.Oxygen.BaseVol, PosTower.D_Fuel.Oxygen.QtyVol);
                fuel_use += GetVolumeForFuel(mech_part, PosTower.D_Fuel.MechPart.BaseVol, PosTower.D_Fuel.MechPart.QtyVol);
                fuel_use += GetVolumeForFuel(coolant, PosTower.D_Fuel.Coolant.BaseVol, PosTower.D_Fuel.Coolant.QtyVol);
                fuel_use += GetVolumeForFuel(robotic, PosTower.D_Fuel.Robotics.BaseVol, PosTower.D_Fuel.Robotics.QtyVol);
                fuel_use += GetVolumeForFuel(hvy_water, PosTower.D_Fuel.HvyWater.BaseVol, PosTower.D_Fuel.HvyWater.QtyVol);
                fuel_use += GetVolumeForFuel(liq_ozone, PosTower.D_Fuel.LiqOzone.BaseVol, PosTower.D_Fuel.LiqOzone.QtyVol);
                fuel_use += GetVolumeForFuel(charter, PosTower.D_Fuel.Charters.BaseVol, PosTower.D_Fuel.Charters.QtyVol);
                fuel_use += GetVolumeForFuel(n2_iso, PosTower.D_Fuel.N2Iso.BaseVol, PosTower.D_Fuel.N2Iso.QtyVol);
                fuel_use += GetVolumeForFuel(h2_iso, PosTower.D_Fuel.H2Iso.BaseVol, PosTower.D_Fuel.H2Iso.QtyVol);
                fuel_use += GetVolumeForFuel(o2_iso, PosTower.D_Fuel.O2Iso.BaseVol, PosTower.D_Fuel.O2Iso.QtyVol);
                fuel_use += GetVolumeForFuel(he_iso, PosTower.D_Fuel.HeIso.BaseVol, PosTower.D_Fuel.HeIso.QtyVol);
            }

            return (period -1);
        }

        public void CalculatePOSDesignFuelValues()
        {
            decimal enr_uran = 0, oxygen = 0, mech_part = 0, robotic = 0;
            decimal coolant = 0, hvy_water = 0, liq_ozone = 0, stront = 0;
            decimal charter = 0, h2_iso = 0, n2_iso = 0, he_iso = 0, o2_iso = 0;
            decimal m_fb_use = 0, m_sb_use = 0;
            decimal cpu, cpu_used, power, power_used;
            decimal cpu_mod, power_mod;
            decimal period, s_period;

            period = ComputeMaxPosRunTimeForLoad();
            switch ((int)PosTower.Design_Interval)
            {
                case 0: // Hours
                    PosTower.Design_Int_Qty = period;
                    break;
                case 1: // Days
                    PosTower.Design_Int_Qty = Math.Floor(period / 24);
                    period = PosTower.Design_Int_Qty * 24;
                    break;
                case 2: // Weeks
                    PosTower.Design_Int_Qty = Math.Floor(period / (24 * 7));
                    period = PosTower.Design_Int_Qty * (7 * 24);
                    break;
                case 3: // Maximum Run
                    PosTower.Design_Int_Qty = period;
                    break;
                default:
                    period = 1;
                    break;
            }

            // Get Base POS Tower Fuel Numbers - ie: Amount per Period (1 hour typically)
            enr_uran = PosTower.D_Fuel.EnrUran.PeriodQty;
            oxygen = PosTower.D_Fuel.Oxygen.PeriodQty;
            mech_part = PosTower.D_Fuel.MechPart.PeriodQty;
            coolant = PosTower.D_Fuel.Coolant.PeriodQty;
            robotic = PosTower.D_Fuel.Robotics.PeriodQty;
            hvy_water = PosTower.D_Fuel.HvyWater.PeriodQty;
            liq_ozone = PosTower.D_Fuel.LiqOzone.PeriodQty;
            charter = PosTower.D_Fuel.Charters.PeriodQty;
            n2_iso = PosTower.D_Fuel.N2Iso.PeriodQty;
            he_iso = PosTower.D_Fuel.HeIso.PeriodQty;
            h2_iso = PosTower.D_Fuel.H2Iso.PeriodQty;
            o2_iso = PosTower.D_Fuel.O2Iso.PeriodQty;
            stront = PosTower.D_Fuel.Strontium.PeriodQty;

            s_period = PosTower.Design_Stront_Qty;

            // Modify POS Fuel Numbers on amount (multiplicative)
            enr_uran = enr_uran * period;
            oxygen = oxygen * period;
            mech_part = mech_part * period;
            coolant = coolant * period;
            robotic = robotic * period;
            charter = charter * period;
            n2_iso = n2_iso * period;
            he_iso = he_iso * period;
            h2_iso = h2_iso * period;
            o2_iso = o2_iso * period;
            stront = stront * s_period;
            
            // Determine actual CPU and POWER Fuel qty #s based upon CPU and Power usage of design
            cpu = PosTower.CPU;
            cpu_used = PosTower.CPU_Used;

            power = PosTower.Power;
            power_used = PosTower.Power_Used;

            cpu_mod = GetModifierTime(cpu_used, cpu);
            power_mod = GetModifierTime(power_used, power);

            hvy_water = Math.Ceiling(hvy_water * cpu_mod);
            liq_ozone = Math.Ceiling(liq_ozone * power_mod);
            hvy_water = hvy_water * period;
            liq_ozone = liq_ozone * period;
            
            // Compute Fuel Run Times
            PosTower.D_Fuel.EnrUran.RunTime = period;
            PosTower.D_Fuel.Oxygen.RunTime = period;
            PosTower.D_Fuel.MechPart.RunTime = period;
            PosTower.D_Fuel.Coolant.RunTime = period;
            PosTower.D_Fuel.Robotics.RunTime = period;
            PosTower.D_Fuel.HvyWater.RunTime = period;
            PosTower.D_Fuel.LiqOzone.RunTime = period;
            PosTower.D_Fuel.Charters.RunTime = period;
            PosTower.D_Fuel.N2Iso.RunTime = period;
            PosTower.D_Fuel.HeIso.RunTime = period;
            PosTower.D_Fuel.H2Iso.RunTime = period;
            PosTower.D_Fuel.O2Iso.RunTime = period;

            // Calculate Fuel Bay Volume (and Stront) based on Individual Fuel Volumes.
            m_fb_use = 0;
            m_fb_use += GetVolumeForFuel(enr_uran, PosTower.D_Fuel.EnrUran.BaseVol, PosTower.D_Fuel.EnrUran.QtyVol);
            m_fb_use += GetVolumeForFuel(oxygen, PosTower.D_Fuel.Oxygen.BaseVol, PosTower.D_Fuel.Oxygen.QtyVol);
            m_fb_use += GetVolumeForFuel(mech_part, PosTower.D_Fuel.MechPart.BaseVol, PosTower.D_Fuel.MechPart.QtyVol);
            m_fb_use += GetVolumeForFuel(coolant, PosTower.D_Fuel.Coolant.BaseVol, PosTower.D_Fuel.Coolant.QtyVol);
            m_fb_use += GetVolumeForFuel(robotic, PosTower.D_Fuel.Robotics.BaseVol, PosTower.D_Fuel.Robotics.QtyVol);
            m_fb_use += GetVolumeForFuel(hvy_water, PosTower.D_Fuel.HvyWater.BaseVol, PosTower.D_Fuel.HvyWater.QtyVol);
            m_fb_use += GetVolumeForFuel(liq_ozone, PosTower.D_Fuel.LiqOzone.BaseVol, PosTower.D_Fuel.LiqOzone.QtyVol);
            m_fb_use += GetVolumeForFuel(charter, PosTower.D_Fuel.Charters.BaseVol, PosTower.D_Fuel.Charters.QtyVol);
            m_fb_use += GetVolumeForFuel(n2_iso, PosTower.D_Fuel.N2Iso.BaseVol, PosTower.D_Fuel.N2Iso.QtyVol);
            m_fb_use += GetVolumeForFuel(h2_iso, PosTower.D_Fuel.H2Iso.BaseVol, PosTower.D_Fuel.H2Iso.QtyVol);
            m_fb_use += GetVolumeForFuel(o2_iso, PosTower.D_Fuel.O2Iso.BaseVol, PosTower.D_Fuel.O2Iso.QtyVol);
            m_fb_use += GetVolumeForFuel(he_iso, PosTower.D_Fuel.HeIso.BaseVol, PosTower.D_Fuel.HeIso.QtyVol);
            m_sb_use = GetVolumeForFuel(stront, PosTower.D_Fuel.Strontium.BaseVol, PosTower.D_Fuel.Strontium.QtyVol);

            // Store new POS Fuel Data Values
            PosTower.D_Fuel.EnrUran.Qty = enr_uran;
            PosTower.D_Fuel.Oxygen.Qty = oxygen;
            PosTower.D_Fuel.MechPart.Qty = mech_part;
            PosTower.D_Fuel.Coolant.Qty = coolant;
            PosTower.D_Fuel.Robotics.Qty = robotic;
            PosTower.D_Fuel.HvyWater.Qty = hvy_water;
            PosTower.D_Fuel.LiqOzone.Qty = liq_ozone;
            PosTower.D_Fuel.Charters.Qty = charter;
            PosTower.D_Fuel.N2Iso.Qty = n2_iso;
            PosTower.D_Fuel.HeIso.Qty = he_iso;
            PosTower.D_Fuel.H2Iso.Qty = h2_iso;
            PosTower.D_Fuel.O2Iso.Qty = o2_iso;
            PosTower.D_Fuel.Strontium.Qty = stront;

            PosTower.D_Fuel.FuelUsed = m_fb_use;
            PosTower.D_Fuel.StrontUsed = m_sb_use;
        }

        public void CalculatePOSAdjustRunTime()
        {
            decimal enr_uran = 0, oxygen = 0, mech_part = 0, robotic = 0;
            decimal coolant = 0, hvy_water = 0, liq_ozone = 0, stront = 0;
            decimal charter = 0, h2_iso = 0, n2_iso = 0, he_iso = 0, o2_iso = 0;
            decimal m_fb_use = 0, m_sb_use = 0;
            decimal m_enr_uran = 0, m_oxygen = 0, m_mech_part = 0, m_robotic = 0;
            decimal m_coolant = 0, m_hvy_water = 0, m_liq_ozone = 0, m_stront = 0;
            decimal m_charter = 0, m_h2_iso = 0, m_n2_iso = 0, m_he_iso = 0, m_o2_iso = 0;
            decimal cpu, cpu_used, power, power_used;
            decimal cpu_mod, power_mod;

            // Get Base Monitored POS Tower Fuel Amounts - ie: Amount per Period (1 hour typically)
            enr_uran = PosTower.Fuel.EnrUran.PeriodQty;
            oxygen = PosTower.Fuel.Oxygen.PeriodQty;
            mech_part = PosTower.Fuel.MechPart.PeriodQty;
            coolant = PosTower.Fuel.Coolant.PeriodQty;
            robotic = PosTower.Fuel.Robotics.PeriodQty;
            hvy_water = PosTower.Fuel.HvyWater.PeriodQty;
            liq_ozone = PosTower.Fuel.LiqOzone.PeriodQty;
            charter = PosTower.Fuel.Charters.PeriodQty;
            n2_iso = PosTower.Fuel.N2Iso.PeriodQty;
            he_iso = PosTower.Fuel.HeIso.PeriodQty;
            h2_iso = PosTower.Fuel.H2Iso.PeriodQty;
            o2_iso = PosTower.Fuel.O2Iso.PeriodQty;
            stront = PosTower.Fuel.Strontium.PeriodQty;

            // Get Adjusted POS Fuel Values (current Fuel + adjustment)
            m_enr_uran = PosTower.Fuel.EnrUran.Qty + PosTower.A_Fuel.EnrUran.Qty;
            m_oxygen = PosTower.Fuel.Oxygen.Qty + PosTower.A_Fuel.Oxygen.Qty;
            m_mech_part = PosTower.Fuel.MechPart.Qty + PosTower.A_Fuel.MechPart.Qty;
            m_coolant = PosTower.Fuel.Coolant.Qty + PosTower.A_Fuel.Coolant.Qty;
            m_robotic = PosTower.Fuel.Robotics.Qty + PosTower.A_Fuel.Robotics.Qty;
            m_hvy_water = PosTower.Fuel.HvyWater.Qty + PosTower.A_Fuel.HvyWater.Qty;
            m_liq_ozone = PosTower.Fuel.LiqOzone.Qty + PosTower.A_Fuel.LiqOzone.Qty;
            m_charter = PosTower.Fuel.Charters.Qty + PosTower.A_Fuel.Charters.Qty;
            m_n2_iso = PosTower.Fuel.N2Iso.Qty + PosTower.A_Fuel.N2Iso.Qty;
            m_he_iso = PosTower.Fuel.HeIso.Qty + PosTower.A_Fuel.HeIso.Qty;
            m_h2_iso = PosTower.Fuel.H2Iso.Qty + PosTower.A_Fuel.H2Iso.Qty;
            m_o2_iso = PosTower.Fuel.O2Iso.Qty + PosTower.A_Fuel.O2Iso.Qty;
            m_stront = PosTower.Fuel.Strontium.Qty + PosTower.A_Fuel.Strontium.Qty;

            // Determine actual CPU and POWER Fuel usage
            cpu = PosTower.CPU;
            cpu_used = PosTower.CPU_Used;

            power = PosTower.Power;
            power_used = PosTower.Power_Used;

            cpu_mod = GetModifierTime(cpu_used, cpu);
            power_mod = GetModifierTime(power_used, power);

            hvy_water = Math.Ceiling(hvy_water * cpu_mod);
            liq_ozone = Math.Ceiling(liq_ozone * power_mod);

            // Compute Fuel Run Times
            PosTower.A_Fuel.EnrUran.RunTime = GetFuelRunTime(m_enr_uran, enr_uran);
            PosTower.A_Fuel.Oxygen.RunTime = GetFuelRunTime(m_oxygen, oxygen);
            PosTower.A_Fuel.MechPart.RunTime = GetFuelRunTime(m_mech_part, mech_part);
            PosTower.A_Fuel.Coolant.RunTime = GetFuelRunTime(m_coolant, coolant);
            PosTower.A_Fuel.Robotics.RunTime = GetFuelRunTime(m_robotic, robotic);
            PosTower.A_Fuel.HvyWater.RunTime = GetFuelRunTime(m_hvy_water, hvy_water);
            PosTower.A_Fuel.LiqOzone.RunTime = GetFuelRunTime(m_liq_ozone, liq_ozone);
            PosTower.A_Fuel.Charters.RunTime = GetFuelRunTime(m_charter, charter);
            PosTower.A_Fuel.Strontium.RunTime = GetFuelRunTime(m_stront, stront);
            PosTower.A_Fuel.N2Iso.RunTime = GetFuelRunTime(m_n2_iso, n2_iso);
            PosTower.A_Fuel.O2Iso.RunTime = GetFuelRunTime(m_o2_iso, o2_iso);
            PosTower.A_Fuel.H2Iso.RunTime = GetFuelRunTime(m_h2_iso, h2_iso);
            PosTower.A_Fuel.HeIso.RunTime = GetFuelRunTime(m_he_iso, he_iso);

            // Calculate Fuel Bay Volume (and Stront) based on Individual Fuel Volumes.
            m_fb_use = 0;
            m_fb_use += GetVolumeForFuel(m_enr_uran, PosTower.A_Fuel.EnrUran.BaseVol, PosTower.A_Fuel.EnrUran.QtyVol);
            m_fb_use += GetVolumeForFuel(m_oxygen, PosTower.A_Fuel.Oxygen.BaseVol, PosTower.A_Fuel.Oxygen.QtyVol);
            m_fb_use += GetVolumeForFuel(m_mech_part, PosTower.A_Fuel.MechPart.BaseVol, PosTower.A_Fuel.MechPart.QtyVol);
            m_fb_use += GetVolumeForFuel(m_coolant, PosTower.A_Fuel.Coolant.BaseVol, PosTower.A_Fuel.Coolant.QtyVol);
            m_fb_use += GetVolumeForFuel(m_robotic, PosTower.A_Fuel.Robotics.BaseVol, PosTower.A_Fuel.Robotics.QtyVol);
            m_fb_use += GetVolumeForFuel(m_hvy_water, PosTower.A_Fuel.HvyWater.BaseVol, PosTower.A_Fuel.HvyWater.QtyVol);
            m_fb_use += GetVolumeForFuel(m_liq_ozone, PosTower.A_Fuel.LiqOzone.BaseVol, PosTower.A_Fuel.LiqOzone.QtyVol);
            m_fb_use += GetVolumeForFuel(m_charter, PosTower.A_Fuel.Charters.BaseVol, PosTower.A_Fuel.Charters.QtyVol);
            m_fb_use += GetVolumeForFuel(m_n2_iso, PosTower.A_Fuel.N2Iso.BaseVol, PosTower.A_Fuel.N2Iso.QtyVol);
            m_fb_use += GetVolumeForFuel(m_h2_iso, PosTower.A_Fuel.H2Iso.BaseVol, PosTower.A_Fuel.H2Iso.QtyVol);
            m_fb_use += GetVolumeForFuel(m_o2_iso, PosTower.A_Fuel.O2Iso.BaseVol, PosTower.A_Fuel.O2Iso.QtyVol);
            m_fb_use += GetVolumeForFuel(m_he_iso, PosTower.A_Fuel.HeIso.BaseVol, PosTower.A_Fuel.HeIso.QtyVol);
            m_sb_use = GetVolumeForFuel(m_stront, PosTower.A_Fuel.Strontium.BaseVol, PosTower.A_Fuel.Strontium.QtyVol);

            // Store new POS Fuel Data Values
            PosTower.A_Fuel.EnrUran.Qty = m_enr_uran;
            PosTower.A_Fuel.Oxygen.Qty = m_oxygen;
            PosTower.A_Fuel.MechPart.Qty = m_mech_part;
            PosTower.A_Fuel.Coolant.Qty = m_coolant;
            PosTower.A_Fuel.Robotics.Qty = m_robotic;
            PosTower.A_Fuel.HvyWater.Qty = m_hvy_water;
            PosTower.A_Fuel.LiqOzone.Qty = m_liq_ozone;
            PosTower.A_Fuel.Charters.Qty = m_charter;
            PosTower.A_Fuel.N2Iso.Qty = m_n2_iso;
            PosTower.A_Fuel.HeIso.Qty = m_he_iso;
            PosTower.A_Fuel.H2Iso.Qty = m_h2_iso;
            PosTower.A_Fuel.O2Iso.Qty = m_o2_iso;
            PosTower.A_Fuel.Strontium.Qty = m_stront;

            PosTower.A_Fuel.FuelUsed = m_fb_use;
            PosTower.A_Fuel.StrontUsed = m_sb_use;
        }
    }
}
