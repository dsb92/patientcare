﻿using System;
using PatientCare.Shared.Model;
using PatientCare.Shared.Util;

namespace PatientCare.Shared
{
    /// <summary>
    /// Statisk klasse for håndtering af at wrappe en Category med evt. type og evt. tilebør til et faktisk kald med en Status på kaldet
    /// </summary>
    public static class CallWrapper
    {
        /// <summary>
        /// Wrapper en Category, Choice og/eller Detail til et kald sammen med Patients CPR-nr og en Status kode på kaldet(Afventende, Udført, Fortrudt)
        /// </summary>
        /// <param name="cprnr">CPR-nr for Patient</param>
        /// <param name="status">Status kode(Active, Completed, Canceled) på kaldet</param>
        /// <param name="categoryEntity">Kategorien</param>
        /// <param name="choiceEntity">Valget</param>
        /// <param name="detailEntity">Tilebør</param>
        /// <returns>Returner et objekt der anses som et kald, klar til at blive sendt afsted til Web API</returns>
        public static CallEntity WrapCall(String cprnr, CallUtil.StatusCode status, CategoryEntity categoryEntity, ChoiceEntity choiceEntity=null, DetailEntity detailEntity=null)
        {
            var callEntity = new CallEntity();

            /* Non-nullable values */
            //callEntity._id = Guid.NewGuid().ToString();
            callEntity.PatientCPR = cprnr;
            callEntity.Status = (int)status;
            callEntity.Category = categoryEntity.Name;

            if (choiceEntity != null)
            {
                callEntity.Choice = choiceEntity.Name;
            }

            /* Nullable values*/
            if (detailEntity != null)
            {
                callEntity.Detail = detailEntity.Name;
            }

            return callEntity;

        }

        // For Android
        public static CallEntity WrapCall(String cprnr, CallUtil.StatusCode status, String categoryEntity, String choiceEntity = null, String detailEntity = null)
        {
            var callEntity = new CallEntity();

            /* Non-nullable values */
            //callEntity._id = Guid.NewGuid().ToString();
            callEntity.PatientCPR = cprnr;
            callEntity.Status = (int)status;
            callEntity.Category = categoryEntity;

            if (choiceEntity != null)
            {
                callEntity.Choice = choiceEntity;
            }

            /* Nullable values*/
            if (detailEntity != null)
            {
                callEntity.Detail = detailEntity;
            }

            return callEntity;

        }
    }
}