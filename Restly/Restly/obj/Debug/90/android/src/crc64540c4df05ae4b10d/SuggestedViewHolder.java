package crc64540c4df05ae4b10d;


public class SuggestedViewHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Restly.Activities.SuggestedViewHolder, Restly", SuggestedViewHolder.class, __md_methods);
	}


	public SuggestedViewHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == SuggestedViewHolder.class)
			mono.android.TypeManager.Activate ("Restly.Activities.SuggestedViewHolder, Restly", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
