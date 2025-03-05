using TheAmazingExcavatorRentTool.Services;

namespace TheAmazingExcavatorRentTool.ViewModels;

public class MainWindowVM: BaseVM
{

    private object _currentView;

    public object CurrentView
    {
        get { return _currentView; }
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }
    
    
    public RelayCommand ToBrandCommand { get; set; }
    public RelayCommand ToUserCommand { get; set; }
    public RelayCommand ToExcavatorCommand { get; set; }
    public RelayCommand ToRentalCommand { get; set; }
    
    
    
    
    // Creating all view models
    
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
    
    private CustomerVM _customervm;
        
    public CustomerVM CustomerVm
    {
        get { return _customervm; }
        set { _customervm = value; }
    }
    
    private RentalVM _rentalvm;
        
    public RentalVM RentalVm
    {
        get { return _rentalvm; }
        set { _rentalvm = value; }
    }
    
        
    private UserVM _uservm;
        
    public UserVM UserVm
    {
        get { return _uservm; }
        set { _uservm = value; }
    }

    public MainWindowVM()
    {
        BrandVm = new BrandVM();
        ExcavatorVm = new ExcavatorVM(BrandVm);
        CustomerVm = new CustomerVM();
        RentalVm = new RentalVM(customervm: CustomerVm, excavatorvm: ExcavatorVm);
        if (Session.IsAdmin)
        {
            UserVm = new UserVM();   
        }

        // Loading commands
        ToBrandCommand = new RelayCommand(o =>
        {
            CurrentView = BrandVm;
        });
        
        ToUserCommand = new RelayCommand(o =>
        {
            CurrentView = UserVm;
        });
        
        ToExcavatorCommand = new RelayCommand(o =>
        {
            CurrentView = ExcavatorVm;
        });
        
        ToRentalCommand = new RelayCommand(o =>
        {
            CurrentView = RentalVm;
        });
        
        CurrentView = ExcavatorVm;
    }
}