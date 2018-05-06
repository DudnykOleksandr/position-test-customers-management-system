CustomerViewModel = function () {

    //Make the self as 'this' reference
    var self = this;
    //Declare observable which will be bind with UI
    self.customerId = ko.observable("");
    self.name = ko.observable("");

    self.contacts = ko.observableArray([]);

    self.newContact = ko.observable(null);

    self.addNewContact = function () {
        self.newContact(new ContactViewModel());
    }
    self.saveNewContact = function () {
        self.contacts.push(self.newContact());
        self.newContact(null);
    }

    self.toJson = function () {
        var result = {};
        var contacts = self.contacts().map(contact => contact.toJson());
        result.CustomerId = self.customerId();
        result.Name = self.name();
        result.Contacts = contacts;
        return result;
    };

    self.populate = function (data) {
        self.customerId(data.customerId);
        self.name(data.name);
    };

    //Add New Item
    self.create = function () {
        if (Product.Name() != "" && Product.Price() != "" && Product.Category() != "") {
            $.ajax({
                url: 'Product/AddProduct',
                cache: false,
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: ko.toJSON(Product),
                success: function (data) {
                    self.Products.push(data);
                    self.Name("");
                    self.Price("");
                    self.Category("");
                }
            }).fail(
                function (xhr, textStatus, err) {
                    alert(err);
                });
        }
        else {
            alert('Please Enter All the Values !!');
        }
    }
};
