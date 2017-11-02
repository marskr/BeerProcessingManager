package md56f98bac558e04f705be3eeeeb28b6152;


public class ViewPageListenerForActionBar
	extends android.support.v4.view.ViewPager.SimpleOnPageChangeListener
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onPageSelected:(I)V:GetOnPageSelected_IHandler\n" +
			"";
		mono.android.Runtime.register ("BeerProcessingManager.ViewPageListenerForActionBar, BeerProcessingManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ViewPageListenerForActionBar.class, __md_methods);
	}


	public ViewPageListenerForActionBar ()
	{
		super ();
		if (getClass () == ViewPageListenerForActionBar.class)
			mono.android.TypeManager.Activate ("BeerProcessingManager.ViewPageListenerForActionBar, BeerProcessingManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public ViewPageListenerForActionBar (android.app.ActionBar p0)
	{
		super ();
		if (getClass () == ViewPageListenerForActionBar.class)
			mono.android.TypeManager.Activate ("BeerProcessingManager.ViewPageListenerForActionBar, BeerProcessingManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.App.ActionBar, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public void onPageSelected (int p0)
	{
		n_onPageSelected (p0);
	}

	private native void n_onPageSelected (int p0);

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
