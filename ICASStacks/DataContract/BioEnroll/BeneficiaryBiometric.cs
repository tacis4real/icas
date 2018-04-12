namespace ICASStacks.DataContract.BioEnroll
{
    public class BeneficiaryBiometric
    {
        public long BeneficiaryBiometricId { get; set; }
        public long BeneficiaryId { get; set; }
        public string RightThumbPrintImage { get; set; }
        public string RightIndexPrintImage { get; set; }
        public byte[] RightThumbPrintTemplate { get; set; }
        public byte[] RightIndexPrintTemplate { get; set; }



        #region Navigation Properties
        public virtual Beneficiary Beneficiary { get; set; }
        #endregion


    }
}
