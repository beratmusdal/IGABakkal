using IGAMarket.EntityLayer.Concrete;

namespace IGAMarket.BusinessLayer.Abstract;

public interface ISepetService : IGenericService<Sepet>
{
    List<Sepet> IncreaseOrAdd(Sepet entity);
    List<Sepet> DecreaseOrDelete(string barcode);
    List<Sepet> DeleteThenGetList(string barcode);
    void RemoveAll();
}
