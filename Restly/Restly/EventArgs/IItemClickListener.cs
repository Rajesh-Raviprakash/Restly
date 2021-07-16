using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace Restly
{
    public interface IItemClickListener
    {
        void OnClick(View itemView, int position, bool isLongClick, CardView categoryCardview, TextView categoryTitle);
    }

    public interface IRadioClickListener
    {
        void OnClick(View itemView, int position, bool isLongClick, RadioButton radiobutton);
    }
}