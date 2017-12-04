using GradHelperWPF.Views;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GradHelperWPF.ViewModel
{
	/**
	 * 
	 * 
	 * */
	public class MajorFormViewModel : BindableBase
	{
        public GradAppViewModel GradApp
        {
            get { return GradApplicationView.gradAppViewModelStatic; }
        }

        public MajorFormViewModel()
        {


        }

        public String Row1_Col1_Dept     { get { return _engr010[0];    } set { SetProperty(ref _engr010[0], value); } }
        public String Row1_Col1_No       { get { return _engr010[1];    } set { SetProperty(ref _engr010[1], value); } }
        public String Row1_Col1_Title    { get { return _engr010[2];    } set { SetProperty(ref _engr010[2], value); } }
        public String Row1_Col1_Units    { get { return _engr010[3];    } set { SetProperty(ref _engr010[3], value); } }
        public String Row1_Col1_Grade    { get { return _engr010[4];    } set { SetProperty(ref _engr010[4], value); } }

        public String Row1_Col2_Dept     { get { return _cs046A[0];     } set { SetProperty(ref _cs046A[0], value); } }
        public String Row1_Col2_No       { get { return _cs046A[1];     } set { SetProperty(ref _cs046A[1], value); } }
        public String Row1_Col2_Title    { get { return _cs046A[2];     } set { SetProperty(ref _cs046A[2], value); } }
        public String Row1_Col2_Units    { get { return _cs046A[3];     } set { SetProperty(ref _cs046A[3], value); } }
        public String Row1_Col2_Grade    { get { return _cs046A[4];     } set { SetProperty(ref _cs046A[4], value); } }

        public String Row2_Col1_Dept     { get { return ""; } }
        public String Row2_Col1_No       { get { return ""; } }
        public String Row2_Col1_Title    { get { return ""; } }
        public String Row2_Col1_Units	 { get { return ""; } }
        public String Row2_Col1_Grade    { get { return ""; } } 

        public String Row2_Col2_Dept     { get { return _cs046B[0];    }  set { SetProperty(ref _cs046B[0], value); } }
        public String Row2_Col2_No       { get { return _cs046B[1];    }  set { SetProperty(ref _cs046B[1], value); } }
        public String Row2_Col2_Title    { get { return _cs046B[2];    }  set { SetProperty(ref _cs046B[2], value); } }
        public String Row2_Col2_Units    { get { return _cs046B[3];    }  set { SetProperty(ref _cs046B[3], value); } }
        public String Row2_Col2_Grade    { get { return _cs046B[4];    }  set { SetProperty(ref _cs046B[4], value); } }

        public String Row3_Col1_Dept     { get { return _cmpe102[0];     }  set { SetProperty(ref _cmpe102[0], value); } }
        public String Row3_Col1_No       { get { return _cmpe102[1];     }  set { SetProperty(ref _cmpe102[1], value); } }
        public String Row3_Col1_Title    { get { return _cmpe102[2];     }  set { SetProperty(ref _cmpe102[2], value); } }
        public String Row3_Col1_Units    { get { return _cmpe102[3];     }  set { SetProperty(ref _cmpe102[3], value); } }
        public String Row3_Col1_Grade    { get { return _cmpe102[4];     }  set { SetProperty(ref _cmpe102[4], value); } }

        public String Row3_Col2_Dept     { get { return _cs149[0];    }  set { SetProperty(ref _cs149[0], value); } }
        public String Row3_Col2_No       { get { return _cs149[1];    }  set { SetProperty(ref _cs149[1], value); } }
        public String Row3_Col2_Title    { get { return _cs149[2];    }  set { SetProperty(ref _cs149[2], value); } }
        public String Row3_Col2_Units    { get { return _cs149[3];    }  set { SetProperty(ref _cs149[3], value); } }
        public String Row3_Col2_Grade    { get { return _cs149[4];    }  set { SetProperty(ref _cs149[4], value); } }

        public String Row4_Col1_Dept     { get { return _cmpe120[0];     }  set { SetProperty(ref _cmpe120[0], value); } }
        public String Row4_Col1_No       { get { return _cmpe120[1];     }  set { SetProperty(ref _cmpe120[1], value); } }
        public String Row4_Col1_Title    { get { return _cmpe120[2];     }  set { SetProperty(ref _cmpe120[2], value); } }
        public String Row4_Col1_Units    { get { return _cmpe120[3];     }  set { SetProperty(ref _cmpe120[3], value); } }
        public String Row4_Col1_Grade    { get { return _cmpe120[4];     }  set { SetProperty(ref _cmpe120[4], value); } }

        public String Row4_Col2_Dept     { get { return _cs151[0];    }  set { SetProperty(ref _cs151[0], value); } }
        public String Row4_Col2_No       { get { return _cs151[1];    }  set { SetProperty(ref _cs151[1], value); } }
        public String Row4_Col2_Title    { get { return _cs151[2];    }  set { SetProperty(ref _cs151[2], value); } }
        public String Row4_Col2_Units    { get { return _cs151[3];    }  set { SetProperty(ref _cs151[3], value); } }
        public String Row4_Col2_Grade    { get { return _cs151[4];    }  set { SetProperty(ref _cs151[4], value); } }

        public String Row5_Col1_Dept     { get { return _cmpe131[0];     }  set { SetProperty(ref _cmpe131[0], value); } }
        public String Row5_Col1_No       { get { return _cmpe131[1];     }  set { SetProperty(ref _cmpe131[1], value); } }
        public String Row5_Col1_Title    { get { return _cmpe131[2];     }  set { SetProperty(ref _cmpe131[2], value); } }
        public String Row5_Col1_Units    { get { return _cmpe131[3];     }  set { SetProperty(ref _cmpe131[3], value); } }
        public String Row5_Col1_Grade    { get { return _cmpe131[4];     }  set { SetProperty(ref _cmpe131[4], value); } }

        public String Row5_Col2_Dept     { get { return _cs157A[0];    }  set { SetProperty(ref _cs157A[0], value); } }
        public String Row5_Col2_No       { get { return _cs157A[1];    }  set { SetProperty(ref _cs157A[1], value); } }
        public String Row5_Col2_Title    { get { return _cs157A[2];    }  set { SetProperty(ref _cs157A[2], value); } }
        public String Row5_Col2_Units    { get { return _cs157A[3];    }  set { SetProperty(ref _cs157A[3], value); } }
        public String Row5_Col2_Grade    { get { return _cs157A[4];    }  set { SetProperty(ref _cs157A[4], value); } }

        public String Row6_Col1_Dept     { get { return _cmpe133[0];     }  set { SetProperty(ref _cmpe133[0], value); } }
        public String Row6_Col1_No       { get { return _cmpe133[1];     }  set { SetProperty(ref _cmpe133[1], value); } }
        public String Row6_Col1_Title    { get { return _cmpe133[2];     }  set { SetProperty(ref _cmpe133[2], value); } }
        public String Row6_Col1_Units    { get { return _cmpe133[3];     }  set { SetProperty(ref _cmpe133[3], value); } }
        public String Row6_Col1_Grade    { get { return _cmpe133[4];     }  set { SetProperty(ref _cmpe133[4], value); } }

        public String Row6_Col2_Dept     { get { return _cs166[0];    }  set { SetProperty(ref _cs166[0], value); } }
        public String Row6_Col2_No       { get { return _cs166[1];    }  set { SetProperty(ref _cs166[1], value); } }
        public String Row6_Col2_Title    { get { return _cs166[2];    }  set { SetProperty(ref _cs166[2], value); } }
        public String Row6_Col2_Units    { get { return _cs166[3];    }  set { SetProperty(ref _cs166[3], value); } }
        public String Row6_Col2_Grade    { get { return _cs166[4];    }  set { SetProperty(ref _cs166[4], value); } }

        public String Row7_Col1_Dept     { get { return _cmpe148[0];     }  set { SetProperty(ref _cmpe148[0], value); } }
        public String Row7_Col1_No       { get { return _cmpe148[1];     }  set { SetProperty(ref _cmpe148[1], value); } }
        public String Row7_Col1_Title    { get { return _cmpe148[2];     }  set { SetProperty(ref _cmpe148[2], value); } }
        public String Row7_Col1_Units    { get { return _cmpe148[3];     }  set { SetProperty(ref _cmpe148[3], value); } }
        public String Row7_Col1_Grade    { get { return _cmpe148[4];     }  set { SetProperty(ref _cmpe148[4], value); } }

        public String Row7_Col2_Dept     { get { return _ise164[0];    }  set { SetProperty(ref _ise164[0], value); } }
        public String Row7_Col2_No       { get { return _ise164[1];    }  set { SetProperty(ref _ise164[1], value); } }
        public String Row7_Col2_Title    { get { return _ise164[2];    }  set { SetProperty(ref _ise164[2], value); } }
        public String Row7_Col2_Units    { get { return _ise164[3];    }  set { SetProperty(ref _ise164[3], value); } }
        public String Row7_Col2_Grade    { get { return _ise164[4];    }  set { SetProperty(ref _ise164[4], value); } }

        public String Row8_Col1_Dept     { get { return _cmpe165[0];     }  set { SetProperty(ref _cmpe165[0], value); } }
        public String Row8_Col1_No       { get { return _cmpe165[1];     }  set { SetProperty(ref _cmpe165[1], value); } }
        public String Row8_Col1_Title    { get { return _cmpe165[2];     }  set { SetProperty(ref _cmpe165[2], value); } }
        public String Row8_Col1_Units    { get { return _cmpe165[3];     }  set { SetProperty(ref _cmpe165[3], value); } }
        public String Row8_Col1_Grade    { get { return _cmpe165[4];     }  set { SetProperty(ref _cmpe165[4], value); } }

        public String Row8_Col2_Dept     { get { return _cmpe195A[0];    }  set { SetProperty(ref _cmpe195A[0], value); } }
        public String Row8_Col2_No       { get { return _cmpe195A[1];    }  set { SetProperty(ref _cmpe195A[1], value); } }
        public String Row8_Col2_Title    { get { return _cmpe195A[2];    }  set { SetProperty(ref _cmpe195A[2], value); } }
        public String Row8_Col2_Units    { get { return _cmpe195A[3];    }  set { SetProperty(ref _cmpe195A[3], value); } }
        public String Row8_Col2_Grade    { get { return _cmpe195A[4];    }  set { SetProperty(ref _cmpe195A[4], value); } }

        public String Row9_Col1_Dept     { get { return _cmpe172[0];     }  set { SetProperty(ref _cmpe172[0], value); } }
        public String Row9_Col1_No       { get { return _cmpe172[1];     }  set { SetProperty(ref _cmpe172[1], value); } }
        public String Row9_Col1_Title    { get { return _cmpe172[2];     }  set { SetProperty(ref _cmpe172[2], value); } }
        public String Row9_Col1_Units    { get { return _cmpe172[3];     }  set { SetProperty(ref _cmpe172[3], value); } }
        public String Row9_Col1_Grade    { get { return _cmpe172[4];     }  set { SetProperty(ref _cmpe172[4], value); } }

        public String Row9_Col2_Dept     { get { return _cmpe195B[0];    }  set { SetProperty(ref _cmpe195B[0], value); } }
        public String Row9_Col2_No       { get { return _cmpe195B[1];    }  set { SetProperty(ref _cmpe195B[1], value); } }
        public String Row9_Col2_Title    { get { return _cmpe195B[2];    }  set { SetProperty(ref _cmpe195B[2], value); } }
        public String Row9_Col2_Units    { get { return _cmpe195B[3];    }  set { SetProperty(ref _cmpe195B[3], value); } }
        public String Row9_Col2_Grade    { get { return _cmpe195B[4];    }  set { SetProperty(ref _cmpe195B[4], value); } }

        public String Row10_Col1_Dept    { get { return _cmpe187[0];     }  set { SetProperty(ref _cmpe187[0], value); } }
        public String Row10_Col1_No      { get { return _cmpe187[1];     }  set { SetProperty(ref _cmpe187[1], value); } }
        public String Row10_Col1_Title   { get { return _cmpe187[2];     }  set { SetProperty(ref _cmpe187[2], value); } }
        public String Row10_Col1_Units   { get { return _cmpe187[3];     }  set { SetProperty(ref _cmpe187[3], value); } }
        public String Row10_Col1_Grade   { get { return _cmpe187[4];     }  set { SetProperty(ref _cmpe187[4], value); } }

        public String Row10_Col2_Dept    { get { return _engr195A[0];    } set { SetProperty(ref _engr195A[0], value); } }
        public String Row10_Col2_No      { get { return _engr195A[1];    } set { SetProperty(ref _engr195A[1], value); } }
        public String Row10_Col2_Title   { get { return _engr195A[2];    } set { SetProperty(ref _engr195A[2], value); } }
        public String Row10_Col2_Units   { get { return _engr195A[3];    } set { SetProperty(ref _engr195A[3], value); } }
        public String Row10_Col2_Grade   { get { return _engr195A[4];    } set { SetProperty(ref _engr195A[4], value); } }

        public String Row11_Col1_Dept    { get { return _cs146[0];     } set { SetProperty(ref _cs146[0], value); } }
        public String Row11_Col1_No      { get { return _cs146[1];     } set { SetProperty(ref _cs146[1], value); } }
        public String Row11_Col1_Title   { get { return _cs146[2];     } set { SetProperty(ref _cs146[2], value); } }
        public String Row11_Col1_Units   { get { return _cs146[3];     } set { SetProperty(ref _cs146[3], value); } }
        public String Row11_Col1_Grade   { get { return _cs146[4];     } set { SetProperty(ref _cs146[4], value); } }

        public String Row11_Col2_Dept    {  get { return _engr195B[0];     } set { SetProperty(ref _engr195B[0], value); } }
        public String Row11_Col2_No      {  get { return _engr195B[1];     } set { SetProperty(ref _engr195B[1], value); } }
        public String Row11_Col2_Title   {  get { return _engr195B[2];     } set { SetProperty(ref _engr195B[2], value); } }
        public String Row11_Col2_Units   {  get { return _engr195B[3];     } set { SetProperty(ref _engr195B[3], value); } }
        public String Row11_Col2_Grade   {  get { return _engr195B[4];     } set { SetProperty(ref _engr195B[4], value); } }

        //Tech electives
        public String Row12_Col1_Dept     { get { return _tech1[0];     } set { SetProperty(ref _tech1[0], value); } }
        public String Row12_Col1_No       { get { return _tech1[1];     } set { SetProperty(ref _tech1[1], value); } }
        public String Row12_Col1_Title    { get { return _tech1[2];     } set { SetProperty(ref _tech1[2], value); } }
        public String Row12_Col1_Units    { get { return _tech1[3];     } set { SetProperty(ref _tech1[3], value); } }
        public String Row12_Col1_Grade    { get { return _tech1[4];     } set { SetProperty(ref _tech1[4], value); } }
														  
        public String Row12_Col2_Dept     { get { return _tech2[0];     } set { SetProperty(ref _tech2[0], value); } }
        public String Row12_Col2_No       { get { return _tech2[1];     } set { SetProperty(ref _tech2[1], value); } }
        public String Row12_Col2_Title    { get { return _tech2[2];     } set { SetProperty(ref _tech2[2], value); } }
        public String Row12_Col2_Units    { get { return _tech2[3];     } set { SetProperty(ref _tech2[3], value); } }
        public String Row12_Col2_Grade    { get { return _tech2[4];     } set { SetProperty(ref _tech2[4], value); } }

        public String Row13_Col1_Dept     { get { return _biol010[0];     }  set { SetProperty(ref _biol010[0], value); } }
        public String Row13_Col1_No       { get { return _biol010[1];     }  set { SetProperty(ref _biol010[1], value); } }
        public String Row13_Col1_Title    { get { return _biol010[2];     }  set { SetProperty(ref _biol010[2], value); } }
        public String Row13_Col1_Units    { get { return _biol010[3];     }  set { SetProperty(ref _biol010[3], value); } }
        public String Row13_Col1_Grade    { get { return _biol010[4];     }  set { SetProperty(ref _biol010[4], value); } }

        public String Row13_Col2_Dept     { get { return _phys050[0];     }  set { SetProperty(ref _phys050[0], value); } }
        public String Row13_Col2_No       { get { return _phys050[1];     }  set { SetProperty(ref _phys050[1], value); } }
        public String Row13_Col2_Title    { get { return _phys050[2];     }  set { SetProperty(ref _phys050[2], value); } }
        public String Row13_Col2_Units    { get { return _phys050[3];     }  set { SetProperty(ref _phys050[3], value); } }
        public String Row13_Col2_Grade    { get { return _phys050[4];     }  set { SetProperty(ref _phys050[4], value); } }

        public String Row14_Col1_Dept     { get { return _math030[0];     }  set { SetProperty(ref _math030[0], value); } }
        public String Row14_Col1_No       { get { return _math030[1];     }  set { SetProperty(ref _math030[1], value); } }
        public String Row14_Col1_Title    { get { return _math030[2];     }  set { SetProperty(ref _math030[2], value); } }
        public String Row14_Col1_Units    { get { return _math030[3];     }  set { SetProperty(ref _math030[3], value); } }
        public String Row14_Col1_Grade    { get { return _math030[4];     }  set { SetProperty(ref _math030[4], value); } }

        public String Row14_Col2_Dept     { get { return _phys051[0];     }  set { SetProperty(ref _phys051[0], value); } }
        public String Row14_Col2_No       { get { return _phys051[1];     }  set { SetProperty(ref _phys051[1], value); } }
        public String Row14_Col2_Title    { get { return _phys051[2];     }  set { SetProperty(ref _phys051[2], value); } }
        public String Row14_Col2_Units    { get { return _phys051[3];     }  set { SetProperty(ref _phys051[3], value); } }
        public String Row14_Col2_Grade    { get { return _phys051[4];     }  set { SetProperty(ref _phys051[4], value); } }
        
        public String Row15_Col1_Dept     { get { return _math031[0];     }  set { SetProperty(ref _math031[0], value); } }
        public String Row15_Col1_No       { get { return _math031[1];     }  set { SetProperty(ref _math031[1], value); } }
        public String Row15_Col1_Title    { get { return _math031[2];     }  set { SetProperty(ref _math031[2], value); } }
        public String Row15_Col1_Units    { get { return _math031[3];     }  set { SetProperty(ref _math031[3], value); } }
        public String Row15_Col1_Grade    { get { return _math031[4];     }  set { SetProperty(ref _math031[4], value); } }

        public String Row15_Col2_Dept     { get { return _math123[0];     }  set { SetProperty(ref _math123[0], value); } }
        public String Row15_Col2_No       { get { return _math123[1];     }  set { SetProperty(ref _math123[1], value); } }
        public String Row15_Col2_Title    { get { return _math123[2];     }  set { SetProperty(ref _math123[2], value); } }
        public String Row15_Col2_Units    { get { return _math123[3];     }  set { SetProperty(ref _math123[3], value); } }
        public String Row15_Col2_Grade    { get { return _math123[4];     }  set { SetProperty(ref _math123[4], value); } }
        
        public String Row16_Col1_Dept     { get { return _math032[0];     }  set { SetProperty(ref _math032[0], value); } }
        public String Row16_Col1_No       { get { return _math032[1];     }  set { SetProperty(ref _math032[1], value); } }
        public String Row16_Col1_Title    { get { return _math032[2];     }  set { SetProperty(ref _math032[2], value); } }
        public String Row16_Col1_Units    { get { return _math032[3];     }  set { SetProperty(ref _math032[3], value); } }
        public String Row16_Col1_Grade    { get { return _math032[4];     }  set { SetProperty(ref _math032[4], value); } }

        public String Row16_Col2_Dept     { get { return _ise130[0];     }  set { SetProperty(ref _ise130[0], value); } }
        public String Row16_Col2_No       { get { return _ise130[1];     }  set { SetProperty(ref _ise130[1], value); } }
        public String Row16_Col2_Title    { get { return _ise130[2];     }  set { SetProperty(ref _ise130[2], value); } }
        public String Row16_Col2_Units    { get { return _ise130[3];     }  set { SetProperty(ref _ise130[3], value); } }
        public String Row16_Col2_Grade    { get { return _ise130[4];     }  set { SetProperty(ref _ise130[4], value); } }
        
        public String Row17_Col1_Dept     { get { return _math042[0];     } set { SetProperty(ref _math042[0], value); } }
        public String Row17_Col1_No       { get { return _math042[1];     } set { SetProperty(ref _math042[1], value); } }
        public String Row17_Col1_Title    { get { return _math042[2];     } set { SetProperty(ref _math042[2], value); } }
        public String Row17_Col1_Units    { get { return _math042[3];     } set { SetProperty(ref _math042[3], value); } }
        public String Row17_Col1_Grade    { get { return _math042[4];     } set { SetProperty(ref _math042[4], value); } }

        public String Row17_Col2_Dept     { get { return _engl01B[0];     } set { SetProperty(ref _engl01B[0], value); } }
        public String Row17_Col2_No       { get { return _engl01B[1];     } set { SetProperty(ref _engl01B[1], value); } }
        public String Row17_Col2_Title    { get { return _engl01B[2];     } set { SetProperty(ref _engl01B[2], value); } }
        public String Row17_Col2_Units    { get { return _engl01B[3];     } set { SetProperty(ref _engl01B[3], value); } }
        public String Row17_Col2_Grade    { get { return _engl01B[4];     } set { SetProperty(ref _engl01B[4], value); } }
    
        private readonly string dept = "Dept";
        private readonly string no = "No.";
        private readonly string title = "Title";
        private readonly string units = "Units";
        private readonly string grade = "Grade";

        private String[] _engr010          = new String[6];
        private String[] _engr195A         = new String[6];
        private String[] _engr195B         = new String[6];

        private String[] _cs046A           = new String[6];
        private String[] _cs046B           = new String[6];
        private String[] _cs146            = new String[6];
        private String[] _cs149            = new String[6];
        private String[] _cs151            = new String[6];
        private String[] _cs157A           = new String[6];
        private String[] _cs166            = new String[6];

        private String[] _ise164           = new String[6];

        private String[] _cmpe102          = new String[6];
        private String[] _cmpe120          = new String[6];
        private String[] _cmpe131          = new String[6];
        private String[] _cmpe133          = new String[6];
        private String[] _cmpe148          = new String[6];
        private String[] _cmpe165          = new String[6];
        private String[] _cmpe172          = new String[6];
        private String[] _cmpe187          = new String[6];
        private String[] _cmpe195A         = new String[6];
        private String[] _cmpe195B         = new String[6];

        private String[] _biol010          = new String[6];
        private String[] _math030          = new String[6];
        private String[] _math031          = new String[6];
        private String[] _math032          = new String[6];
        private String[] _math042          = new String[6];

        private String[] _phys050          = new String[6];
        private String[] _phys051          = new String[6];
        private String[] _math123          = new String[6];
        private String[] _ise130           = new String[6];
        private String[] _engl01B          = new String[6];

		private String[] _tech1			   = new String[6];
		private String[] _tech2			   = new String[6];



        /*
         
        private Dictionary<string, string> _engr010          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _engr195A         = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _engr195B         = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };

        private Dictionary<string, string> _cs046A           = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _cs046B           = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _cs149            = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _cs151            = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _cs157A           = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _cs166            = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };

        private Dictionary<string, string> _ise164           = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };

        private Dictionary<string, string> _cmpe102          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _cmpe120          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _cmpe131          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _cmpe133          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _cmpe148          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _cmpe165          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _cmpe172          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _cmpe187          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _cmpe195A         = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _cmpe195B         = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };

        private Dictionary<string, string> _biol010          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _math030          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _math031          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _math032          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _math042          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };

        private Dictionary<string, string> _phys050          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _phys051          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _math123          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _ise130           = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };
        private Dictionary<string, string> _engl01B          = new Dictionary<string, string>()
        {
            {"Dept", "Engr" },
            {"No.", "010" },
            {"Title", "Introduction to Engineering" },
            {"Units", "3" },
            {"Grade", "" },
            {"Row", "1" }
        };

         
         */

    }
}
