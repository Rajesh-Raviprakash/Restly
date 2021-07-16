package crc645c826d028222510a;


public class OptionsDataHolder
	extends android.support.v7.widget.RecyclerView.ViewHolder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Restly.Controls.OptionsDataHolder, Restly", OptionsDataHolder.class, __md_methods);
	}


	public OptionsDataHolder (android.view.View p0)
	{
		super (p0);
		if (getClass () == OptionsDataHolder.class)
			mono.android.TypeManager.Activate ("Restly.Controls.OptionsDataHolder, Restly", "Android.Views.View, Mono.Android", this, new java.lang.Object[] { p0 });
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
