// tripsController.js

(function () {
    "use strict";

    angular.module("app-trips").controller("tripsController", tripsController);

    function tripsController($http) {

        var vm = this;

        //vm.name = "Shawn";

        //vm.trips = [{
        //    name: "US Trips",
        //    created: new Date()
        //}, {
        //    name: "World Trips",
        //    created: new Date()
        //}];
        vm.trips = [];

        vm.newTrip = {};

        vm.errorMessage = "";
        vm.isBusy = true;

        $http.get("/api/trips")
            .then(function (response) {
                // Success
                angular.copy(response.data, vm.trips);
            }, function (error) {
                // Failure
                vm.errorMessage = "Failed to load data: " + error;
            })
            .finally(function () {
                vm.isBusy = false;
            });

        vm.addTrip = function () {
            //alert(vm.newTrip.name);
            //vm.trips.push({ name: vm.newTrip.name, created: new Date() })
            //vm.newTrip = {};

            vm.errorMessage = "";
            vm.isBusy = true;

            $http.post("/api/trips", vm.newTrip)
            .then(function (response) {
                // Success
                vm.trips.push(response.data);
                vm.newTrip = {};
            }, function () {
                // Failure
                vm.errorMessage = "Failed to save new trip!";
            })
            .finally(function () {
                vm.isBusy = false;
            });

        }
    }
})();