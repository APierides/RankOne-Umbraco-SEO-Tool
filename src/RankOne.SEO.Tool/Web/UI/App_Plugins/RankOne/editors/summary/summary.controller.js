﻿(function () {

    // Controller
    function rankOne($scope, $http, editorState, resultService, urlService) {

        $scope.sortOrder = ['-errorCount', '-warningCount', '-hintCount'];

        if (!editorState.current.template) {
            $scope.error = "This item does not have a template";
        } else {
            var relativeUrl = editorState.current.urls[0];

            if (relativeUrl == "This item is not published") {
                $scope.error = "This item is not published";
            } else {
                $scope.loading = true;
                var url = urlService.GetUrl(relativeUrl);

                $http({
                    method: 'GET',
                    url: '/umbraco/backoffice/api/RankOneApi/AnalyzeUrl?url=' + url
                }).then(function successCallback(response) {
                    $scope.analyzeResults = response.data;
                    resultService.SetMetadata($scope.analyzeResults);
                    $scope.loading = false;
                }, function errorCallback(response) {
                    console.log(response);
                });
            }
        }
    };

    // Register the controller
    angular.module("umbraco").controller('rankOneSummary', rankOne);

})();