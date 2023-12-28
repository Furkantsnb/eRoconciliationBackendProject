using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Constans
{
    public static class Messages
    {
        public static string AddedCompany = "Şirket kaydı başarıyla tamamlandı";
        public static string UpdatedCompany = "Şirketler Başarıyla Güncellendi";

        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string UserRegistered = "Kullanıcı Kaydı Başarılı";

        public static string PasswordError = "Şifre Yanlış";
        public static string SuccessfulLogin = "Giriş Başarılı";

       
        public static string UserAlREadyExist = "Bu Kullanıcı Zaten var";
        public static string CompanyAlreadyExists = "Bu şirket daha önce kaydedilmiş";


        public static string MailParameterAdded = "Mail Parametreleri Başarıyla Güncellendi";
        public static string MailSendSucessful = "Mail Gönderme Başarılı";


        public static string MailTemplateAdded = "Mail Şablonu Başarıyla Eklendi";
        public static string MailTemplateDeleted = "Mail Şablonu Başarıyla Silindi";
        public static string MailTemplateUpdated = "Mail Şablonu Başarıyla Güncellendi";


        public static string UserMailConfirmSuccessful = "Mailiniz Başarıyla Onaylandı";
        public static string MailConfirmSendSuccessful = "Onay Maili Tekrar Göderildi";
        public static string MailAlreadConfirm = "Mail Zaten Onaylı. Tekrar Gönderim Yapılmadı.";
        public static string MailConfirmTimeHasNotExpire = "Mail Onayını 5 dk Bir Görebilirsiniz";
       

        public static string AddedCurrencyAccount = "Cari Kaydı Başarıyla Eklendi";
        public static string UpdatedCurrencyAccount = "Cari Kaydı Başarıyla Güncellendi";
        public static string DeletedCurrencyAccount = "Cari Kaydı Başarıyla Silindi";

        public static string AddedAccountReconciliation = "Cari Mutabakat Kaydı Başarıyla eklendi";
        public static string UpdatedAccountReconciliation = "Cari Mutabakat Kaydı Başarıyla Güncellendi";
        public static string DeletedAccountReconciliation = "Cari Kaydı Başarıyla Silindi";

        public static string AddedAccountReconciliationDetail = "Cari Mutabakat detay kaydı başarıyla Eklendi";
        public static string DeletedAccountReconciliationDetail = "Cari Mutabakat detay kaydı başarıyla Silindi";
        public static string UpdatedAccountReconciliationDetail = "Cari Mutabakat detay kaydı başarıyla Güncellendi";

        public static string AddedBaBsReconciliation = "BaBs Kaydı Başarıyla eklendi";
        public static string DeletedBaBsReconciliation = "BaBs Kaydı Başarıyla silindi";
        public static string UpdatedBaBsReconciliation = "BaBs Kaydı Başarıyla güncellendi";

        public static string AddedBaBsReconciliationDetail = "BaBsDetail Kaydı Başarıyla eklendi";
        public static string DeletedBaBsReconciliationDetail = "BaBsDetail Kaydı Başarıyla silindi";
        public static string UpdatedBaBsReconciliationDetail = "BaBsDetail Kaydı Başarıyla güncellendi";

        public static string UpdatedOperationClaim = "Yetki Başarıyla Güncellendi";
        public static string DeletedOperationClaim = "Yetki Başarıyla Silindi";
        public static string AddedOperationClaim = "Yetki Başarıyla Eklendi";

        public static string AddedUserOperationClaim = "Kullanıcıya yetki başarıyla eklendi";
        public static string UpdatedUserOperationClaim = "Kullanıcıya yetki başarıyla güncellendi";
        public static string DeletedUserOperationClaim = "Kullanıcıya yetki başarıyla silindi";
    }
}
