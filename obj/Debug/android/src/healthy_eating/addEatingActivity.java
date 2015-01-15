package healthy_eating;


public class addEatingActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("healthy_eating.addEatingActivity, healthy_eating, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", addEatingActivity.class, __md_methods);
	}


	public addEatingActivity () throws java.lang.Throwable
	{
		super ();
		if (getClass () == addEatingActivity.class)
			mono.android.TypeManager.Activate ("healthy_eating.addEatingActivity, healthy_eating, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	java.util.ArrayList refList;
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
