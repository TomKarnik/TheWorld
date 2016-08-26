// tripEditorController.js

(function () {

    "use strict";

    angular.module("app-trips").controller("tripEditorController", tripEditorController);

    function tripEditorController($routeParams, $http) {
        var vm = this;

        vm.tripName = $routeParams.tripName;

        vm.stops = [];
        vm.errorMessage = "";
        vm.isBusy = true;
        vm.newStop = {};

        $http.get("/api/trips/" + vm.tripName + "/stops")
            .then(function (response) {
                // Success
                angular.copy(response.data, vm.stops);
                _showMap(vm.stops);
            },
            function (err) {
                // Failure
                vm.errorMessage = "Error getting stops!";
            })
            .finally(function () {
                vm.isBusy = false;
            });
    }

    function _showMap(stops) {
        if (stops && stops.length > 0) {

            var mapStops = _.map(stops, function (item) {
                return {
                    lat: item.latitude,
                    long: item.longitude,
                    info: item.name
                };

            });

            // Show Map
            travelMap.createMap({
                stops: mapStops,
                selector: "#map",
                currentStop: 1,
                initialZoom: 3

            });
        }

    }

})();