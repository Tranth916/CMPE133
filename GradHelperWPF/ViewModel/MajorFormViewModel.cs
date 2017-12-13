using GradHelperWPF.Views;
using Prism.Mvvm;

namespace GradHelperWPF.ViewModel
{
    /**
     *
     *
     * */

    public class MajorFormViewModel : BindableBase
    {
        private readonly string[] _biol010 = new string[6];

        private readonly string[] _cmpe102 = new string[6];
        private readonly string[] _cmpe120 = new string[6];
        private readonly string[] _cmpe131 = new string[6];
        private readonly string[] _cmpe133 = new string[6];
        private readonly string[] _cmpe148 = new string[6];
        private readonly string[] _cmpe165 = new string[6];
        private readonly string[] _cmpe172 = new string[6];
        private readonly string[] _cmpe187 = new string[6];
        private readonly string[] _cmpe195A = new string[6];
        private readonly string[] _cmpe195B = new string[6];

        private readonly string[] _cs046A = new string[6];
        private readonly string[] _cs046B = new string[6];
        private readonly string[] _cs146 = new string[6];
        private readonly string[] _cs149 = new string[6];
        private readonly string[] _cs151 = new string[6];
        private readonly string[] _cs157A = new string[6];
        private readonly string[] _cs166 = new string[6];
        private readonly string[] _engl01B = new string[6];

        private readonly string[] _engr010 = new string[6];
        private readonly string[] _engr195A = new string[6];
        private readonly string[] _engr195B = new string[6];
        private readonly string[] _ise130 = new string[6];

        private readonly string[] _ise164 = new string[6];
        private readonly string[] _math030 = new string[6];
        private readonly string[] _math031 = new string[6];
        private readonly string[] _math032 = new string[6];
        private readonly string[] _math042 = new string[6];
        private readonly string[] _math123 = new string[6];

        private readonly string[] _phys050 = new string[6];
        private readonly string[] _phys051 = new string[6];

        private readonly string[] _tech1 = new string[6];
        private readonly string[] _tech2 = new string[6];

        public MajorFormViewModel( )
        {
            Init( );
        }

        public GradAppViewModel GradApp => GradApplicationView.GradAppViewModelStatic;

        public string Row1_Col1_Dept
        {
            get => _engr010[0];
            set => SetProperty( ref _engr010[0], value );
        }

        public string Row1_Col1_No
        {
            get => _engr010[1];
            set => SetProperty( ref _engr010[1], value );
        }

        public string Row1_Col1_Title
        {
            get => _engr010[2];
            set => SetProperty( ref _engr010[2], value );
        }

        public string Row1_Col1_Units
        {
            get => _engr010[3];
            set => SetProperty( ref _engr010[3], value );
        }

        public string Row1_Col1_Grade
        {
            get => _engr010[4];
            set => SetProperty( ref _engr010[4], value );
        }

        public string Row1_Col2_Dept
        {
            get => _cs046A[0];
            set => SetProperty( ref _cs046A[0], value );
        }

        public string Row1_Col2_No
        {
            get => _cs046A[1];
            set => SetProperty( ref _cs046A[1], value );
        }

        public string Row1_Col2_Title
        {
            get => _cs046A[2];
            set => SetProperty( ref _cs046A[2], value );
        }

        public string Row1_Col2_Units
        {
            get => _cs046A[3];
            set => SetProperty( ref _cs046A[3], value );
        }

        public string Row1_Col2_Grade
        {
            get => _cs046A[4];
            set => SetProperty( ref _cs046A[4], value );
        }

        public string Row2_Col1_Dept
        {
            get => "";
            set => SetProperty( ref _cs046A[0], value );
        }

        public string Row2_Col1_No
        {
            get => "";
            set => SetProperty( ref _cs046A[1], value );
        }

        public string Row2_Col1_Title
        {
            get => "";
            set => SetProperty( ref _cs046A[2], value );
        }

        public string Row2_Col1_Units
        {
            get => "";
            set => SetProperty( ref _cs046A[3], value );
        }

        public string Row2_Col1_Grade
        {
            get => "";
            set => SetProperty( ref _cs046A[4], value );
        }

        public string Row2_Col2_Dept
        {
            get => _cs046B[0];
            set => SetProperty( ref _cs046B[0], value );
        }

        public string Row2_Col2_No
        {
            get => _cs046B[1];
            set => SetProperty( ref _cs046B[1], value );
        }

        public string Row2_Col2_Title
        {
            get => _cs046B[2];
            set => SetProperty( ref _cs046B[2], value );
        }

        public string Row2_Col2_Units
        {
            get => _cs046B[3];
            set => SetProperty( ref _cs046B[3], value );
        }

        public string Row2_Col2_Grade
        {
            get => _cs046B[4];
            set => SetProperty( ref _cs046B[4], value );
        }

        public string Row3_Col1_Dept
        {
            get => _cmpe102[0];
            set => SetProperty( ref _cmpe102[0], value );
        }

        public string Row3_Col1_No
        {
            get => _cmpe102[1];
            set => SetProperty( ref _cmpe102[1], value );
        }

        public string Row3_Col1_Title
        {
            get => _cmpe102[2];
            set => SetProperty( ref _cmpe102[2], value );
        }

        public string Row3_Col1_Units
        {
            get => _cmpe102[3];
            set => SetProperty( ref _cmpe102[3], value );
        }

        public string Row3_Col1_Grade
        {
            get => _cmpe102[4];
            set => SetProperty( ref _cmpe102[4], value );
        }

        public string Row3_Col2_Dept
        {
            get => _cs149[0];
            set => SetProperty( ref _cs149[0], value );
        }

        public string Row3_Col2_No
        {
            get => _cs149[1];
            set => SetProperty( ref _cs149[1], value );
        }

        public string Row3_Col2_Title
        {
            get => _cs149[2];
            set => SetProperty( ref _cs149[2], value );
        }

        public string Row3_Col2_Units
        {
            get => _cs149[3];
            set => SetProperty( ref _cs149[3], value );
        }

        public string Row3_Col2_Grade
        {
            get => _cs149[4];
            set => SetProperty( ref _cs149[4], value );
        }

        public string Row4_Col1_Dept
        {
            get => _cmpe120[0];
            set => SetProperty( ref _cmpe120[0], value );
        }

        public string Row4_Col1_No
        {
            get => _cmpe120[1];
            set => SetProperty( ref _cmpe120[1], value );
        }

        public string Row4_Col1_Title
        {
            get => _cmpe120[2];
            set => SetProperty( ref _cmpe120[2], value );
        }

        public string Row4_Col1_Units
        {
            get => _cmpe120[3];
            set => SetProperty( ref _cmpe120[3], value );
        }

        public string Row4_Col1_Grade
        {
            get => _cmpe120[4];
            set => SetProperty( ref _cmpe120[4], value );
        }

        public string Row4_Col2_Dept
        {
            get => _cs151[0];
            set => SetProperty( ref _cs151[0], value );
        }

        public string Row4_Col2_No
        {
            get => _cs151[1];
            set => SetProperty( ref _cs151[1], value );
        }

        public string Row4_Col2_Title
        {
            get => _cs151[2];
            set => SetProperty( ref _cs151[2], value );
        }

        public string Row4_Col2_Units
        {
            get => _cs151[3];
            set => SetProperty( ref _cs151[3], value );
        }

        public string Row4_Col2_Grade
        {
            get => _cs151[4];
            set => SetProperty( ref _cs151[4], value );
        }

        public string Row5_Col1_Dept
        {
            get => _cmpe131[0];
            set => SetProperty( ref _cmpe131[0], value );
        }

        public string Row5_Col1_No
        {
            get => _cmpe131[1];
            set => SetProperty( ref _cmpe131[1], value );
        }

        public string Row5_Col1_Title
        {
            get => _cmpe131[2];
            set => SetProperty( ref _cmpe131[2], value );
        }

        public string Row5_Col1_Units
        {
            get => _cmpe131[3];
            set => SetProperty( ref _cmpe131[3], value );
        }

        public string Row5_Col1_Grade
        {
            get => _cmpe131[4];
            set => SetProperty( ref _cmpe131[4], value );
        }

        public string Row5_Col2_Dept
        {
            get => _cs157A[0];
            set => SetProperty( ref _cs157A[0], value );
        }

        public string Row5_Col2_No
        {
            get => _cs157A[1];
            set => SetProperty( ref _cs157A[1], value );
        }

        public string Row5_Col2_Title
        {
            get => _cs157A[2];
            set => SetProperty( ref _cs157A[2], value );
        }

        public string Row5_Col2_Units
        {
            get => _cs157A[3];
            set => SetProperty( ref _cs157A[3], value );
        }

        public string Row5_Col2_Grade
        {
            get => _cs157A[4];
            set => SetProperty( ref _cs157A[4], value );
        }

        public string Row6_Col1_Dept
        {
            get => _cmpe133[0];
            set => SetProperty( ref _cmpe133[0], value );
        }

        public string Row6_Col1_No
        {
            get => _cmpe133[1];
            set => SetProperty( ref _cmpe133[1], value );
        }

        public string Row6_Col1_Title
        {
            get => _cmpe133[2];
            set => SetProperty( ref _cmpe133[2], value );
        }

        public string Row6_Col1_Units
        {
            get => _cmpe133[3];
            set => SetProperty( ref _cmpe133[3], value );
        }

        public string Row6_Col1_Grade
        {
            get => _cmpe133[4];
            set => SetProperty( ref _cmpe133[4], value );
        }

        public string Row6_Col2_Dept
        {
            get => _cs166[0];
            set => SetProperty( ref _cs166[0], value );
        }

        public string Row6_Col2_No
        {
            get => _cs166[1];
            set => SetProperty( ref _cs166[1], value );
        }

        public string Row6_Col2_Title
        {
            get => _cs166[2];
            set => SetProperty( ref _cs166[2], value );
        }

        public string Row6_Col2_Units
        {
            get => _cs166[3];
            set => SetProperty( ref _cs166[3], value );
        }

        public string Row6_Col2_Grade
        {
            get => _cs166[4];
            set => SetProperty( ref _cs166[4], value );
        }

        public string Row7_Col1_Dept
        {
            get => _cmpe148[0];
            set => SetProperty( ref _cmpe148[0], value );
        }

        public string Row7_Col1_No
        {
            get => _cmpe148[1];
            set => SetProperty( ref _cmpe148[1], value );
        }

        public string Row7_Col1_Title
        {
            get => _cmpe148[2];
            set => SetProperty( ref _cmpe148[2], value );
        }

        public string Row7_Col1_Units
        {
            get => _cmpe148[3];
            set => SetProperty( ref _cmpe148[3], value );
        }

        public string Row7_Col1_Grade
        {
            get => _cmpe148[4];
            set => SetProperty( ref _cmpe148[4], value );
        }

        public string Row7_Col2_Dept
        {
            get => _ise164[0];
            set => SetProperty( ref _ise164[0], value );
        }

        public string Row7_Col2_No
        {
            get => _ise164[1];
            set => SetProperty( ref _ise164[1], value );
        }

        public string Row7_Col2_Title
        {
            get => _ise164[2];
            set => SetProperty( ref _ise164[2], value );
        }

        public string Row7_Col2_Units
        {
            get => _ise164[3];
            set => SetProperty( ref _ise164[3], value );
        }

        public string Row7_Col2_Grade
        {
            get => _ise164[4];
            set => SetProperty( ref _ise164[4], value );
        }

        public string Row8_Col1_Dept
        {
            get => _cmpe165[0];
            set => SetProperty( ref _cmpe165[0], value );
        }

        public string Row8_Col1_No
        {
            get => _cmpe165[1];
            set => SetProperty( ref _cmpe165[1], value );
        }

        public string Row8_Col1_Title
        {
            get => _cmpe165[2];
            set => SetProperty( ref _cmpe165[2], value );
        }

        public string Row8_Col1_Units
        {
            get => _cmpe165[3];
            set => SetProperty( ref _cmpe165[3], value );
        }

        public string Row8_Col1_Grade
        {
            get => _cmpe165[4];
            set => SetProperty( ref _cmpe165[4], value );
        }

        public string Row8_Col2_Dept
        {
            get => _cmpe195A[0];
            set => SetProperty( ref _cmpe195A[0], value );
        }

        public string Row8_Col2_No
        {
            get => _cmpe195A[1];
            set => SetProperty( ref _cmpe195A[1], value );
        }

        public string Row8_Col2_Title
        {
            get => _cmpe195A[2];
            set => SetProperty( ref _cmpe195A[2], value );
        }

        public string Row8_Col2_Units
        {
            get => _cmpe195A[3];
            set => SetProperty( ref _cmpe195A[3], value );
        }

        public string Row8_Col2_Grade
        {
            get => _cmpe195A[4];
            set => SetProperty( ref _cmpe195A[4], value );
        }

        public string Row9_Col1_Dept
        {
            get => _cmpe172[0];
            set => SetProperty( ref _cmpe172[0], value );
        }

        public string Row9_Col1_No
        {
            get => _cmpe172[1];
            set => SetProperty( ref _cmpe172[1], value );
        }

        public string Row9_Col1_Title
        {
            get => _cmpe172[2];
            set => SetProperty( ref _cmpe172[2], value );
        }

        public string Row9_Col1_Units
        {
            get => _cmpe172[3];
            set => SetProperty( ref _cmpe172[3], value );
        }

        public string Row9_Col1_Grade
        {
            get => _cmpe172[4];
            set => SetProperty( ref _cmpe172[4], value );
        }

        public string Row9_Col2_Dept
        {
            get => _cmpe195B[0];
            set => SetProperty( ref _cmpe195B[0], value );
        }

        public string Row9_Col2_No
        {
            get => _cmpe195B[1];
            set => SetProperty( ref _cmpe195B[1], value );
        }

        public string Row9_Col2_Title
        {
            get => _cmpe195B[2];
            set => SetProperty( ref _cmpe195B[2], value );
        }

        public string Row9_Col2_Units
        {
            get => _cmpe195B[3];
            set => SetProperty( ref _cmpe195B[3], value );
        }

        public string Row9_Col2_Grade
        {
            get => _cmpe195B[4];
            set => SetProperty( ref _cmpe195B[4], value );
        }

        public string Row10_Col1_Dept
        {
            get => _cmpe187[0];
            set => SetProperty( ref _cmpe187[0], value );
        }

        public string Row10_Col1_No
        {
            get => _cmpe187[1];
            set => SetProperty( ref _cmpe187[1], value );
        }

        public string Row10_Col1_Title
        {
            get => _cmpe187[2];
            set => SetProperty( ref _cmpe187[2], value );
        }

        public string Row10_Col1_Units
        {
            get => _cmpe187[3];
            set => SetProperty( ref _cmpe187[3], value );
        }

        public string Row10_Col1_Grade
        {
            get => _cmpe187[4];
            set => SetProperty( ref _cmpe187[4], value );
        }

        public string Row10_Col2_Dept
        {
            get => _engr195A[0];
            set => SetProperty( ref _engr195A[0], value );
        }

        public string Row10_Col2_No
        {
            get => _engr195A[1];
            set => SetProperty( ref _engr195A[1], value );
        }

        public string Row10_Col2_Title
        {
            get => _engr195A[2];
            set => SetProperty( ref _engr195A[2], value );
        }

        public string Row10_Col2_Units
        {
            get => _engr195A[3];
            set => SetProperty( ref _engr195A[3], value );
        }

        public string Row10_Col2_Grade
        {
            get => _engr195A[4];
            set => SetProperty( ref _engr195A[4], value );
        }

        public string Row11_Col1_Dept
        {
            get => _cs146[0];
            set => SetProperty( ref _cs146[0], value );
        }

        public string Row11_Col1_No
        {
            get => _cs146[1];
            set => SetProperty( ref _cs146[1], value );
        }

        public string Row11_Col1_Title
        {
            get => _cs146[2];
            set => SetProperty( ref _cs146[2], value );
        }

        public string Row11_Col1_Units
        {
            get => _cs146[3];
            set => SetProperty( ref _cs146[3], value );
        }

        public string Row11_Col1_Grade
        {
            get => _cs146[4];
            set => SetProperty( ref _cs146[4], value );
        }

        public string Row11_Col2_Dept
        {
            get => _engr195B[0];
            set => SetProperty( ref _engr195B[0], value );
        }

        public string Row11_Col2_No
        {
            get => _engr195B[1];
            set => SetProperty( ref _engr195B[1], value );
        }

        public string Row11_Col2_Title
        {
            get => _engr195B[2];
            set => SetProperty( ref _engr195B[2], value );
        }

        public string Row11_Col2_Units
        {
            get => _engr195B[3];
            set => SetProperty( ref _engr195B[3], value );
        }

        public string Row11_Col2_Grade
        {
            get => _engr195B[4];
            set => SetProperty( ref _engr195B[4], value );
        }

        //Tech electives
        public string Row12_Col1_Dept
        {
            get => _tech1[0];
            set => SetProperty( ref _tech1[0], value );
        }

        public string Row12_Col1_No
        {
            get => _tech1[1];
            set => SetProperty( ref _tech1[1], value );
        }

        public string Row12_Col1_Title
        {
            get => _tech1[2];
            set => SetProperty( ref _tech1[2], value );
        }

        public string Row12_Col1_Units
        {
            get => _tech1[3];
            set => SetProperty( ref _tech1[3], value );
        }

        public string Row12_Col1_Grade
        {
            get => _tech1[4];
            set => SetProperty( ref _tech1[4], value );
        }

        public string Row12_Col2_Dept
        {
            get => _tech2[0];
            set => SetProperty( ref _tech2[0], value );
        }

        public string Row12_Col2_No
        {
            get => _tech2[1];
            set => SetProperty( ref _tech2[1], value );
        }

        public string Row12_Col2_Title
        {
            get => _tech2[2];
            set => SetProperty( ref _tech2[2], value );
        }

        public string Row12_Col2_Units
        {
            get => _tech2[3];
            set => SetProperty( ref _tech2[3], value );
        }

        public string Row12_Col2_Grade
        {
            get => _tech2[4];
            set => SetProperty( ref _tech2[4], value );
        }

        public string Row13_Col1_Dept
        {
            get => _biol010[0];
            set => SetProperty( ref _biol010[0], value );
        }

        public string Row13_Col1_No
        {
            get => _biol010[1];
            set => SetProperty( ref _biol010[1], value );
        }

        public string Row13_Col1_Title
        {
            get => _biol010[2];
            set => SetProperty( ref _biol010[2], value );
        }

        public string Row13_Col1_Units
        {
            get => _biol010[3];
            set => SetProperty( ref _biol010[3], value );
        }

        public string Row13_Col1_Grade
        {
            get => _biol010[4];
            set => SetProperty( ref _biol010[4], value );
        }

        public string Row13_Col2_Dept
        {
            get => _phys050[0];
            set => SetProperty( ref _phys050[0], value );
        }

        public string Row13_Col2_No
        {
            get => _phys050[1];
            set => SetProperty( ref _phys050[1], value );
        }

        public string Row13_Col2_Title
        {
            get => _phys050[2];
            set => SetProperty( ref _phys050[2], value );
        }

        public string Row13_Col2_Units
        {
            get => _phys050[3];
            set => SetProperty( ref _phys050[3], value );
        }

        public string Row13_Col2_Grade
        {
            get => _phys050[4];
            set => SetProperty( ref _phys050[4], value );
        }

        public string Row14_Col1_Dept
        {
            get => _math030[0];
            set => SetProperty( ref _math030[0], value );
        }

        public string Row14_Col1_No
        {
            get => _math030[1];
            set => SetProperty( ref _math030[1], value );
        }

        public string Row14_Col1_Title
        {
            get => _math030[2];
            set => SetProperty( ref _math030[2], value );
        }

        public string Row14_Col1_Units
        {
            get => _math030[3];
            set => SetProperty( ref _math030[3], value );
        }

        public string Row14_Col1_Grade
        {
            get => _math030[4];
            set => SetProperty( ref _math030[4], value );
        }

        public string Row14_Col2_Dept
        {
            get => _phys051[0];
            set => SetProperty( ref _phys051[0], value );
        }

        public string Row14_Col2_No
        {
            get => _phys051[1];
            set => SetProperty( ref _phys051[1], value );
        }

        public string Row14_Col2_Title
        {
            get => _phys051[2];
            set => SetProperty( ref _phys051[2], value );
        }

        public string Row14_Col2_Units
        {
            get => _phys051[3];
            set => SetProperty( ref _phys051[3], value );
        }

        public string Row14_Col2_Grade
        {
            get => _phys051[4];
            set => SetProperty( ref _phys051[4], value );
        }

        public string Row15_Col1_Dept
        {
            get => _math031[0];
            set => SetProperty( ref _math031[0], value );
        }

        public string Row15_Col1_No
        {
            get => _math031[1];
            set => SetProperty( ref _math031[1], value );
        }

        public string Row15_Col1_Title
        {
            get => _math031[2];
            set => SetProperty( ref _math031[2], value );
        }

        public string Row15_Col1_Units
        {
            get => _math031[3];
            set => SetProperty( ref _math031[3], value );
        }

        public string Row15_Col1_Grade
        {
            get => _math031[4];
            set => SetProperty( ref _math031[4], value );
        }

        public string Row15_Col2_Dept
        {
            get => _math123[0];
            set => SetProperty( ref _math123[0], value );
        }

        public string Row15_Col2_No
        {
            get => _math123[1];
            set => SetProperty( ref _math123[1], value );
        }

        public string Row15_Col2_Title
        {
            get => _math123[2];
            set => SetProperty( ref _math123[2], value );
        }

        public string Row15_Col2_Units
        {
            get => _math123[3];
            set => SetProperty( ref _math123[3], value );
        }

        public string Row15_Col2_Grade
        {
            get => _math123[4];
            set => SetProperty( ref _math123[4], value );
        }

        public string Row16_Col1_Dept
        {
            get => _math032[0];
            set => SetProperty( ref _math032[0], value );
        }

        public string Row16_Col1_No
        {
            get => _math032[1];
            set => SetProperty( ref _math032[1], value );
        }

        public string Row16_Col1_Title
        {
            get => _math032[2];
            set => SetProperty( ref _math032[2], value );
        }

        public string Row16_Col1_Units
        {
            get => _math032[3];
            set => SetProperty( ref _math032[3], value );
        }

        public string Row16_Col1_Grade
        {
            get => _math032[4];
            set => SetProperty( ref _math032[4], value );
        }

        public string Row16_Col2_Dept
        {
            get => _ise130[0];
            set => SetProperty( ref _ise130[0], value );
        }

        public string Row16_Col2_No
        {
            get => _ise130[1];
            set => SetProperty( ref _ise130[1], value );
        }

        public string Row16_Col2_Title
        {
            get => _ise130[2];
            set => SetProperty( ref _ise130[2], value );
        }

        public string Row16_Col2_Units
        {
            get => _ise130[3];
            set => SetProperty( ref _ise130[3], value );
        }

        public string Row16_Col2_Grade
        {
            get => _ise130[4];
            set => SetProperty( ref _ise130[4], value );
        }

        public string Row17_Col1_Dept
        {
            get => _math042[0];
            set => SetProperty( ref _math042[0], value );
        }

        public string Row17_Col1_No
        {
            get => _math042[1];
            set => SetProperty( ref _math042[1], value );
        }

        public string Row17_Col1_Title
        {
            get => _math042[2];
            set => SetProperty( ref _math042[2], value );
        }

        public string Row17_Col1_Units
        {
            get => _math042[3];
            set => SetProperty( ref _math042[3], value );
        }

        public string Row17_Col1_Grade
        {
            get => _math042[4];
            set => SetProperty( ref _math042[4], value );
        }

        public string Row17_Col2_Dept
        {
            get => _engl01B[0];
            set => SetProperty( ref _engl01B[0], value );
        }

        public string Row17_Col2_No
        {
            get => _engl01B[1];
            set => SetProperty( ref _engl01B[1], value );
        }

        public string Row17_Col2_Title
        {
            get => _engl01B[2];
            set => SetProperty( ref _engl01B[2], value );
        }

        public string Row17_Col2_Units
        {
            get => _engl01B[3];
            set => SetProperty( ref _engl01B[3], value );
        }

        public string Row17_Col2_Grade
        {
            get => _engl01B[4];
            set => SetProperty( ref _engl01B[4], value );
        }

        private void Init( )
        {
            for ( var i = 0; i < _engr010.Length; i++ )
            {
                _engr010[i] = "";
                _engr195A[i] = "";
                _engr195B[i] = "";
                _cs046A[i] = "";
                _cs046B[i] = "";
                _cs146[i] = "";
                _cs149[i] = "";
                _cs151[i] = "";
                _cs157A[i] = "";
                _cs166[i] = "";
                _ise164[i] = "";
                _cmpe102[i] = "";
                _cmpe120[i] = "";
                _cmpe131[i] = "";
                _cmpe133[i] = "";
                _cmpe148[i] = "";
                _cmpe165[i] = "";
                _cmpe172[i] = "";
                _cmpe187[i] = "";
                _cmpe195A[i] = "";
                _cmpe195B[i] = "";
                _biol010[i] = "";
                _math030[i] = "";
                _math031[i] = "";
                _math032[i] = "";
                _math042[i] = "";
                _phys050[i] = "";
                _phys051[i] = "";
                _math123[i] = "";
                _ise130[i] = "";
                _engl01B[i] = "";
                _tech1[i] = "";
                _tech2[i] = "";
            }
        }
    }
}