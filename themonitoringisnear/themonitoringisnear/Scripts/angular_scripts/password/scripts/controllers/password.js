angular.module('passwordApp')

	.controller('PasswordCtrl', function($scope, Password) {

		$scope.$watch('user.password', function(pass) {

		    $scope.passwordStrength = Password.getStrength(pass);

		    if ($scope.passwordStrength == 0)
		    {
		        $scope.strength = "";
		    }
		    else if ($scope.passwordStrength >= 1 && $scope.passwordStrength < 40)
		    {
		        $scope.strength = "Weak";
		    }
		    else if ($scope.passwordStrength >= 40 && $scope.passwordStrength <= 70)
		    {
		        $scope.strength = "Good";
		    }
		    else if ($scope.passwordStrength > 70)
		    {
		        $scope.strength = "Strong";
		    }
		    return $scope.strength;
		});


		$scope.isPasswordWeak = function() {

		    return $scope.passwordStrength < 40;
		}

		$scope.isPasswordOk = function() {

			return $scope.passwordStrength >= 40 && $scope.passwordStrength <= 70;
		}

		$scope.isPasswordStrong = function() {

			return $scope.passwordStrength > 70;
		}

		$scope.isInputValid = function(input) {

			return input.$dirty && input.$valid;
		}

		$scope.isInputInvalid = function(input) {
			return input.$dirty && input.$invalid;
		}

	});