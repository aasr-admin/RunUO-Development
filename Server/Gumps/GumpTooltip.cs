/***************************************************************************
 *                               GumpTooltip.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id$
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using Server.Network;

namespace Server.Gumps
{
	public class GumpTooltip : GumpEntry
	{
		private int m_Number;
		private string m_Args;

		public GumpTooltip( int number )
			: this( number, null )
		{
		}

		public GumpTooltip( int number, string args )
		{
			m_Number = number;
			m_Args = args;
		}

		public int Number
		{
			get
			{
				return m_Number;
			}
			set
			{
				Delta( ref m_Number, value );
			}
		}

		public string Args
		{
			get
			{
				return m_Args;
			}
			set
			{
				Delta( ref m_Args, value );
			}
		}

		public override string Compile()
		{
			if ( String.IsNullOrEmpty( m_Args ) )
				return $"{{ tooltip { m_Number } }}";

			return $"{{ tooltip { m_Number } @{ m_Args }@ }}";
		}

		private static byte[] m_LayoutName = Gump.StringToBuffer( "tooltip" );

		public override void AppendTo( IGumpWriter disp )
		{
			disp.AppendLayout( m_LayoutName );
			disp.AppendLayout( m_Number );

			if ( !String.IsNullOrEmpty( m_Args ) )
				disp.AppendLayout( m_Args );
		}
	}
}
