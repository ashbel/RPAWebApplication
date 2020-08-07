var app = angular.module('myApp', ['ngAnimate', 'datatables', 'ngResource']);

app.controller('myController', function ($scope, $http, FileUploadService, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {

      $scope.inventory = [];
      $scope.pharmacies = [];
      $scope.orders = [];
      $scope.payments = [];
      $scope.commision = [];
      $scope.cart = [];
      $scope.locations = [];
      $scope.SelectedFileForUpload = null;
      $scope.totalPayment = 0;
      $scope.totalCommision = 0;
      $scope.users = [];
      $scope.productsdelay = true;
      $scope.ordersdelay = true;
      $scope.paymentsdelay = true;
      $scope.Order = "";
    $scope.show = false;
    $scope.inventoryCount = 0;

        //console.log(response);
        var result = $http.get("/Products/Get");
        result.then(function (data) {
            $scope.inventoryCount = data.data;
            $scope.productsdelay = false;
        });
    
        var pharma = $http.get("/Vendors/Get");
        pharma.then(function(data) {
            //console.log('total: ' + data.data);
            $scope.pharmacies = data.data;
        });

        var orders = $http.get("/Orders/Get");
        orders.then(function(data) {
            $scope.orders = data.data;
            $scope.ordersdelay = false;
        });

        var locations = $http.get("/Surburbs/Get");
        locations.then(function(data) {
            $scope.locations = data.data;
        });

        var payments = $http.get("/Payments/Get");
        payments.then(function(data) {
            $scope.payments = data.data;
            $scope.paymentsdelay = false;
        });
        var commision = $http.get("/Commisions/Get");
        commision.then(function(data) {
            $scope.commision = data.data;
        });

        //var commision = $http.get("/Commisions/Get");
        //commision.success(function (data) {
        //    $scope.commision = data;
        //});

        var users = $http.get("/Admin/Get");
        users.then(function successCallback(data) {
            $scope.users = data.data;
        });

        $scope.totalPayment = function () {
            var total = 0;
            for (count = 0; count < $scope.payments.length; count++) {
                //console.log('total: ' + total);
                total += $scope.payments[count].Amount;
            }
            return total;
        };

        $scope.totalCommision = function () {
            var total = 0;
            for (count = 0; count < $scope.commision.length; count++) {
                console.log('total: ' + total);
                total += $scope.commision[count].Amount;
            }
            return total;
        };

        var findItemById = function (items, Id) {
            return _.find(items, function (item) {
                return item.Id === Id;
            });
        };

        $scope.getCost = function (item) {
            return item.Qty * item.Price;
        };

        $scope.addItem = function (itemToAdd) {
            //console.log(itemToAdd);
            var found = findItemById($scope.cart, itemToAdd.Id);
            if (found) {
                found.Qty += itemToAdd.Qty;
                console.log(found);
            }
            else {
                $scope.cart.push(angular.copy(itemToAdd));
            }
            clearFields();
        };

        $scope.getTotal = function () {
            var total = _.reduce($scope.cart, function (sum, item) {
                return sum + $scope.getCost(item);
            }, 0);
            //console.log('total: ' + total);
            return total;
        };

        $scope.clearCart = function () {
            $scope.cart.length = 0;
            angular.forEach(angular.element("input[type='file']"), function (inputElem) {
                angular.element(inputElem).val(null);
            });
            document.getElementById("customfileupload").innerHTML = "Attach Prescription";

        };

        $scope.removeItem = function (item) {
            var index = $scope.cart.indexOf(item);
            $scope.cart.splice(index, 1);
    };

    $scope.searchItem = function () {
            if ($scope.Vendor && $scope.Name && $scope.Location) {;
                console.log("$scope.Name & $scope.Vendor & $scope.Location");
                $http({
                    url: '/Products/ProductSearch',
                    method: "POST",
                    contentType: "application/json",
                    data: { name: $scope.Name, pharmacy: $scope.Vendor.Name, location: $scope.Location.Name },
                }).then(function onSuccess(response) {
                    // Handle success
                    $scope.inventory = response.data;
                }).catch(function onError(response) {
                    // Handle error
                });
            } else if ($scope.Vendor && $scope.Name) { /**@case2 if only $scope.Vendor query is present**/
                console.log("$scope.Name & $scope.Vendor");
                $http({
                    url: '/Products/ProductSearch',
                    method: "POST",
                    contentType: "application/json",
                    data: { name: $scope.Name, pharmacy: $scope.Vendor.Name },
                }).then(function onSuccess(response) {
                    // Handle success
                    $scope.inventory = response.data;
                }).catch(function onError(response) {
                    // Handle error
                });
            }

            else if ($scope.Name && $scope.Location) { /**@case3 if only $scope.Name query is present**/
                console.log("$scope.Name & $scope.Location");
                $http({
                    url: '/Products/ProductSearch',
                    method: "POST",
                    contentType: "application/json",
                    data: { name: $scope.Name, location: $scope.Location.Name },
                }).then(function onSuccess(response) {
                    // Handle success
                    $scope.inventory = response.data;
                }).catch(function onError(response) {
                    // Handle error
                });

            } else if ($scope.Name) { /**@case3 if only $scope.Name query is present**/
                console.log("In $scope.Name Search" + $scope.Name);
                $http({
                    url: '/Products/ProductSearch',
                    method: "POST",
                    contentType: "application/json",
                    data: { name: $scope.Name },
                }).then(function onSuccess(response) {
                    // Handle success
                    $scope.inventory = response.data;
                }).catch(function onError(response) {
                    // Handle error;
                });
            }

            else {

            }
    };

        $scope.vm = {};
        $scope.vm.dtInstance = {};
        //scope.vm.dtColumnDefs = [DTColumnDefBuilder.newColumnDef(2).notSortable()];
        $scope.vm.dtOptions = DTOptionsBuilder.newOptions()
            .withOption('paging', true)
            .withOption('searching', true)
            .withOption('info', true);
  

        $scope.submit = function (cart) {
            console.log(' Form Submitted ' + JSON.stringify($scope.cart));
            $http({
                url: '/Orders/AddOrder',
                method: "POST",
                contentType: "application/json",
                data: { items : JSON.stringify($scope.cart) },
            }).then(function onSuccess(response) {
                // Handle success
                console.log(response);
                $scope.cart.length = 0;
                var orders = $http.get("/Orders/Get");
                orders.success(function (data) {
                    $scope.orders = data;
                });
            }).catch(function onError(response) {
                // Handle error
                console.log(response);
            });
        };

        //File Select event 
        $scope.selectFileforUpload = function (file) {
            $scope.SelectedFileForUpload = file[0];
        };

        //Save File
        $scope.SaveFile = function () {
            $scope.delay = true; //but I would name it as loading :P
            $scope.IsFormSubmitted = true;
            $scope.Message = "";
            //$scope.ChechFileValid($scope.SelectedFileForUpload);
            //if ($scope.IsFormValid && $scope.IsFileValid) {
            FileUploadService.UploadFile($scope.SelectedFileForUpload, $scope.cart).then(function (d) {
                    //alert(d.Message);
                ClearForm();
                $scope.delay = false;
                var orders = $http.get("/Orders/Get");
                orders.then(function (data) {
                    $scope.orders = data;
                });
                }, function (e) {
                    //alert(e);
                    ClearForm();
                });
            //}
            //else {
            //    $scope.Message = "All the fields are required.";
            //}
        };

        //Clear form 
        function ClearForm() {
            $scope.FileDescription = "";
            $scope.Name = "";
            $scope.Vendor = "";
            $scope.Location = "";
            //as 2 way binding not support for File input Type so we have to clear in this way
            //you can select based on your requirement
            angular.forEach(angular.element("input[type='file']"), function (inputElem) {
                angular.element(inputElem).val(null);
            });

            document.getElementById("customfileupload").innerHTML = "Attach Prescription";

            $scope.cart.length = 0;
            $scope.f1.$setPristine();
            $scope.IsFormSubmitted = false;
        }

        function clearFields() {
            $scope.Name = "";
            $scope.Vendor = "";
            $scope.Location = "";
            $scope.inventory = [];
        }

    });


//filter name customSearch
app.filter('customSearch', ['$http',function ($http) {
    /** @data is the original data**/
    // it is value upon which you have to filter
    /** @Vendor is the search query for Vendor**/
    /** @Name is the search query for Name**/
    return function (data, Vendor, Name, Location) {
        var output = []; // store result in this
        //console.log("Im here");
        /**@case1 if both searches are present**/
        if (Vendor && Name && Location) {
            Vendor = Vendor.toLowerCase();
            Name = Name.toLowerCase();
            Location = Location.toLowerCase();
            //loop over the original array
            for (var i = 0; i < data.length; i++) {
                // check if any result matching the search request
                if (data[i].Vendor.toLowerCase().indexOf(Vendor) !== -1 && data[i].Name.toLowerCase().indexOf(Name) !== -1 && data[i].Location.toLowerCase().indexOf(Location) !== -1) {
                    //push data into results array
                    output.push(data[i]);
                }
            }
        } else if (Vendor && Name) { /**@case2 if only Vendor query is present**/
            Vendor = Vendor.toLowerCase();
            Name = Name.toLowerCase();
            for (var a = 0; a < data.length; a++) {
                if (data[a].Vendor.toLowerCase().indexOf(Vendor) !== -1 && data[a].Name.toLowerCase().indexOf(Name) !== -1) {
                    output.push(data[a]);
                }
            }
        }
        //else if (Vendor && Location) { /**@case3 if only Name query is present**/
        //    Vendor = Vendor.toLowerCase();
        //    Location = Location.toLowerCase();
        //    for (var b = 0; b < data.length; b++) {
        //        if (data[b].Vendor.toLowerCase().indexOf(Vendor) !== -1 && data[b].Location.toLowerCase().indexOf(Location) !== -1) {
        //            output.push(data[b]);
        //        }
        //    }
        //}
        else if (Name && Location) { /**@case3 if only Name query is present**/
            Name = Name.toLowerCase();
            Location = Location.toLowerCase();
            for (var c = 0; c < data.length; c++) {
                if (data[c].Name.toLowerCase().indexOf(Name) !== -1 && data[c].Location.toLowerCase().indexOf(Location) !== -1) {
                    output.push(data[c]);
                }
            }
        } else if (Name) { /**@case3 if only Name query is present**/
            Name = Name.toLowerCase();
            console.log(data);
            //$http({
            //    url: '/Products/ProductSearch',
            //    method: "POST",
            //    contentType: "application/json",
            //    data: { name: Name },
            //}).then(function onSuccess(response) {
            //    // Handle success
            //    console.log(response);
            //    //output = response.Data;
            //}).catch(function onError(response) {
            //    // Handle error
            //    console.log(response);
            //});
            for (var d = 0; d < data.length; d++) {
                if (data[d].Name.toLowerCase().indexOf(Name) !== -1) {
                    output.push(data[d]);
                }
            }
        }
        //else if (Vendor) { /**@case3 if only Name query is present**/
        //    Vendor = Vendor.toLowerCase();
        //    for (var e = 0; e < data.length; e++) {
        //        if (data[e].Vendor.toLowerCase().indexOf(Vendor) !== -1) {
        //            output.push(data[e]);
        //        }
        //    }
        //} else if (Location) { /**@case3 if only Name query is present**/
        //    Location = Location.toLowerCase();
        //    for (var f = 0; f < data.length; f++) {
        //        if (data[f].Location.toLowerCase().indexOf(Location) !== -1) {
        //            output.push(data[f]);
        //        }
        //    }
        //}
        else {
            /**@case4 no query is present**/
          // it is value upon which you have to filter
            //output = data;
        }
        return output; // finally return the result
    }
  }]);


app.factory('FileUploadService', function ($http, $q) { // explained abour controller and service in part 2

    var fac = {};
    fac.UploadFile = function (file, cart) {
        var formData = new FormData();
        formData.append("file", file);
        //We can send more data to server using append   
        console.log(' Form Submitted ' + JSON.stringify(cart));
        formData.append("items", JSON.stringify(cart));

        var defer = $q.defer();
        $http.post("/Orders/CreateOrder", formData,
            {
                withCredentials: false,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            })
            .then(function (d) {
                defer.resolve(d);
            });

        return defer.promise;

    }
    return fac;

});