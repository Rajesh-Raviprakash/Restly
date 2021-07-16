package crc64065f4bdfe5862ff8;


public class AllergenseViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Restly.AllergenseViewHolder, Restly", AllergenseViewHolder.class, __md_methods);
	}


	public AllergenseViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == AllergenseViewHolder.class)
			mono.android.TypeManager.Activate ("Restly.AllergenseViewHolder, Restly", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
