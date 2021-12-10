namespace Server.Engines.Reports
{
	// Modified from MS sample

	//*********************************************************************
	//
	// BarGraph Class
	//
	// This class uses GDI+ to render Bar Chart.
	//
	//*********************************************************************

	public class BarRegion
	{
		public int m_RangeFrom, m_RangeTo;
		public string m_Name;

		public BarRegion( int rangeFrom, int rangeTo, string name )
		{
			m_RangeFrom = rangeFrom;
			m_RangeTo = rangeTo;
			m_Name = name;
		}
	}
}
