using UnityEngine;
using UnityEngine.UI;

namespace GFrame
{
	public class CommonPool : SpawnPool<CommonPool>
	{
		public CommonPool()
		{
			m_Pools.Add("Bar", new BarPool());
			m_Pools.Add("Hero", new HeroPool());
			m_Pools.Add("Loading", new LoadingPool());
		}
	}
	public class BarPool : PrefabPool<Bar>
	{
		public override string PrefabPath()
		{
			return "UIPrefab/Common/Bar";
		}
	}
	public class Bar : UIComponents
	{
		public override void InitUIComponents()
		{
			Hero0_Image = transform.Find("UIGroup/Line/Hero0").GetComponent<Image>();
			Hero1_Image = transform.Find("UIGroup/Line/Hero1").GetComponent<Image>();
			Hero2_Image = transform.Find("UIGroup/Line/Hero2").GetComponent<Image>();
			Hero3_Image = transform.Find("UIGroup/Line/Hero3").GetComponent<Image>();
			Hero4_Image = transform.Find("UIGroup/Line/Hero4").GetComponent<Image>();
		}
		public override void Clear()
		{
			Hero0_Image = null;
			Hero1_Image = null;
			Hero2_Image = null;
			Hero3_Image = null;
			Hero4_Image = null;
		}
		public Image Hero0_Image;
		public Image Hero1_Image;
		public Image Hero2_Image;
		public Image Hero3_Image;
		public Image Hero4_Image;
	}
	public class HeroPool : PrefabPool<Hero>
	{
		public override string PrefabPath()
		{
			return "UIPrefab/Common/Hero";
		}
	}
	public class Hero : UIComponents
	{
		public override void InitUIComponents()
		{
			Hero_Image = transform.Find("Hero").GetComponent<Image>();
			Element_Image = transform.Find("Element").GetComponent<Image>();
			Type_Image = transform.Find("Type").GetComponent<Image>();
		}
		public override void Clear()
		{
			Hero_Image = null;
			Element_Image = null;
			Type_Image = null;
		}
		public Image Hero_Image;
		public Image Element_Image;
		public Image Type_Image;
	}
	public class LoadingPool : PrefabPool<Loading>
	{
		public override string PrefabPath()
		{
			return "UIPrefab/Common/Loading";
		}
	}
	public class Loading : UIComponents
	{
		public override void InitUIComponents()
		{
			Loading_Image = transform.Find("Loading").GetComponent<Image>();
		}
		public override void Clear()
		{
			Loading_Image = null;
		}
		public Image Loading_Image;
	}
}
