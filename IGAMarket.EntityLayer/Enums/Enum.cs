using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGAMarket.EntityLayer.Enums;

public enum CategoryEnum
{
    [Display(Name = "İçecekler")]
    Icecekler = 2,

    [Display(Name = "Kahvaltılıklar")]
    Kahvaltiliklar = 3,

    [Display(Name = "Unlu Mamüller")]
    UnluMamuller = 4,

    [Display(Name = "Sandviçler")]
    Sandvicler = 5,

    [Display(Name = "Fast Food ve Pideler")]
    FastFoodvePideler = 6,

    [Display(Name = "Tatlılar ve Pastalar")]
    TatlilarvePastalar = 7,

    [Display(Name = "Hazır Ürünler ve Atıştırmalıklar")]
    HazirUrunlerveAtistirmaliklar = 8,

    [Display(Name = "Diğer")]
    Diger = 9
}
public enum StatusEnum
{
    Pending = 1,
    Success = 2,
    Fail = 3,
}

