function IndexViewModel() {

    var self = this;

    self.customers = ko.observableArray([]); 

    self.newCustomer = ko.observable(null); 

    self.createNewCustomer = function () {
        self.newCustomer(new CustomerViewModel());
    };

    self.saveNewCustomer = function () {

        var customer = self.newCustomer();
        $.ajax({
            url: 'Customers/CreateFromJson',
            cache: false,
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: ko.toJSON(customer.toJson()),
            success: function (data) {
            }
        });
    };

    // Initialize the view-model
    self.load = function () {
        $.ajax({
            url: 'Customers/GetAllCustomers',
            cache: false,
            type: 'GET',
            contentType: 'application/json; charset=utf-8',
            data: {},
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var customer = new CustomerViewModel();
                    customer.populate(data[i]);
                    self.customers.push(customer);
                }
            }
        });
    };
}
var viewModel = new IndexViewModel();
viewModel.load();

ko.applyBindings(viewModel);
