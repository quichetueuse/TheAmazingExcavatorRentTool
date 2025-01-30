namespace TheAmazingExcavatorRentTool.ViewModels;

public class MainWindowVM: BaseVM
{
    
    
    private ExcavatorVM _excavatorvm;
        
    public ExcavatorVM ExcavatorVm
    {
        get { return _excavatorvm; }
        set { _excavatorvm = value; }
    }
    
    
    
    private BrandVM _brandvm;
        
    public BrandVM BrandVm
    {
        get { return _brandvm; }
        set { _brandvm = value; }
    }
    
        
    // private UserVM _uservm;
    //     
    // public UserVM UserVm
    // {
    //     get { return _uservm; }
    //     set { _uservm = value; }
    // }

    public MainWindowVM()
    {
        BrandVm = new BrandVM();
        ExcavatorVm = new ExcavatorVM(BrandVm);
        // _uservm = new UserVM();
    }
}