// Generated by CoffeeScript 1.7.1
(function() {
  var ProductsViewModel;

  ProductsViewModel = function() {
    var baseUri, obj, self;
    self = this;
    obj = ko.observable();
    self.products = ko.observableArray();
    self.loading = ko.observable(true);
    self.addmode = ko.observable(false);
    self.editMode = ko.observable(false);
    self.name = ko.observable('');
    self.price = ko.observable(0);
    baseUri = "api/Products";
    self.toggleAddMode = function() {
      self.addmode(!self.addmode());
      self.name('');
      self.price(0);
    };
    self.beginEditMode = function(object) {
      obj(object);
    };
    self.endEditMode = function(object) {
      obj(null);
    };
    self.deleteObject = function(object) {
      $.ajax({
        type: "DELETE",
        url: baseUri + "/" + object.Id
      }).done(function(data, textStatus, xhr) {
        self.products.remove(object);
        toastr.success(textStatus);
      }).fail(function(xhr, textStatus, error) {
        toastr.error(error);
      });
    };
    self.updateObject = function(object) {
      $.ajax({
        type: "PUT",
        url: baseUri + "/" + object.Id,
        data: object
      }).done(function(data, textStatus, xhr) {
        toastr.success(textStatus);
        self.endEditMode();
        self.load();
      }).fail(function(xhr, textStatus, error) {
        toastr.error(error);
      });
    };
    self.isEditMode = function(object) {
      return ko.computed(function() {
        return obj() === object;
      }).extend({
        notify: "always"
      })();
    };
    self.create = function(formElement) {
      $(formElement).validate();
      if ($(formElement).valid()) {
        $.post(baseUri, $(formElement).serialize(), null, "json").done(function(o) {
          self.load();
          self.toggleAddMode();
          toastr.success("Adding success");
        }).fail(function(xhr, textStatus, error) {
          toastr.error(error);
        });
      }
    };
    self.load = function() {
      return $.getJSON(baseUri, self.products).done(function() {
        self.loading(false);
      }).fail(function() {
        toastr.error('Loading error');
      });
    };
    self.load();
  };

  $(document).ready(function() {
    ko.applyBindings(new ProductsViewModel());
  });

}).call(this);

//# sourceMappingURL=productKo.map
