using Server.EnumTypes;

namespace Server.ViewModels
{
  public class RequestResultViewModel
  {
    public RequestState State { get; set; }

    public string Msg { get; set; }

    public object Data { get; set; }
  }
}
