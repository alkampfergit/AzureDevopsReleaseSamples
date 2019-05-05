(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["tests-tests-module"],{

/***/ "./src/app/tests/test-web-api/test-web-api.component.css":
/*!***************************************************************!*\
  !*** ./src/app/tests/test-web-api/test-web-api.component.css ***!
  \***************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3Rlc3RzL3Rlc3Qtd2ViLWFwaS90ZXN0LXdlYi1hcGkuY29tcG9uZW50LmNzcyJ9 */"

/***/ }),

/***/ "./src/app/tests/test-web-api/test-web-api.component.html":
/*!****************************************************************!*\
  !*** ./src/app/tests/test-web-api/test-web-api.component.html ***!
  \****************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<h2>Test Web Api</h2>\n\n<input placeholder=\"echo\" [(ngModel)]=\"txt\"><button (click)=\"callEcho()\">Echo!</button>\n<br>\n{{echo | async}}\n"

/***/ }),

/***/ "./src/app/tests/test-web-api/test-web-api.component.ts":
/*!**************************************************************!*\
  !*** ./src/app/tests/test-web-api/test-web-api.component.ts ***!
  \**************************************************************/
/*! exports provided: TestWebApiComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TestWebApiComponent", function() { return TestWebApiComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var src_app_services_myWonderfulAppServices_web__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! src/app/services/myWonderfulAppServices.web */ "./src/app/services/myWonderfulAppServices.web.ts");



var TestWebApiComponent = /** @class */ (function () {
    function TestWebApiComponent(testClient) {
        this.testClient = testClient;
    }
    TestWebApiComponent.prototype.ngOnInit = function () {
    };
    TestWebApiComponent.prototype.callEcho = function () {
        this.echo = this.testClient.ping("1.0");
    };
    TestWebApiComponent = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"])({
            selector: 'app-test-web-api',
            template: __webpack_require__(/*! ./test-web-api.component.html */ "./src/app/tests/test-web-api/test-web-api.component.html"),
            providers: [
                src_app_services_myWonderfulAppServices_web__WEBPACK_IMPORTED_MODULE_2__["SupportClient"]
            ],
            styles: [__webpack_require__(/*! ./test-web-api.component.css */ "./src/app/tests/test-web-api/test-web-api.component.css")]
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [src_app_services_myWonderfulAppServices_web__WEBPACK_IMPORTED_MODULE_2__["SupportClient"]])
    ], TestWebApiComponent);
    return TestWebApiComponent;
}());



/***/ }),

/***/ "./src/app/tests/tests-index/tests-index.component.css":
/*!*************************************************************!*\
  !*** ./src/app/tests/tests-index/tests-index.component.css ***!
  \*************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "\n/*# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IiIsImZpbGUiOiJzcmMvYXBwL3Rlc3RzL3Rlc3RzLWluZGV4L3Rlc3RzLWluZGV4LmNvbXBvbmVudC5jc3MifQ== */"

/***/ }),

/***/ "./src/app/tests/tests-index/tests-index.component.html":
/*!**************************************************************!*\
  !*** ./src/app/tests/tests-index/tests-index.component.html ***!
  \**************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = "<h1>Tests</h1>\n\n<a [routerLink]=\"['./test-web-api']\">Test Web Api</a>\n<br />\n"

/***/ }),

/***/ "./src/app/tests/tests-index/tests-index.component.ts":
/*!************************************************************!*\
  !*** ./src/app/tests/tests-index/tests-index.component.ts ***!
  \************************************************************/
/*! exports provided: TestsIndexComponent */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TestsIndexComponent", function() { return TestsIndexComponent; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");


var TestsIndexComponent = /** @class */ (function () {
    function TestsIndexComponent() {
    }
    TestsIndexComponent.prototype.ngOnInit = function () {
    };
    TestsIndexComponent = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["Component"])({
            selector: 'app-tests-index',
            template: __webpack_require__(/*! ./tests-index.component.html */ "./src/app/tests/tests-index/tests-index.component.html"),
            styles: [__webpack_require__(/*! ./tests-index.component.css */ "./src/app/tests/tests-index/tests-index.component.css")]
        }),
        tslib__WEBPACK_IMPORTED_MODULE_0__["__metadata"]("design:paramtypes", [])
    ], TestsIndexComponent);
    return TestsIndexComponent;
}());



/***/ }),

/***/ "./src/app/tests/tests.module.ts":
/*!***************************************!*\
  !*** ./src/app/tests/tests.module.ts ***!
  \***************************************/
/*! exports provided: TestsModule */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "TestsModule", function() { return TestsModule; });
/* harmony import */ var tslib__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! tslib */ "./node_modules/tslib/tslib.es6.js");
/* harmony import */ var _angular_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @angular/core */ "./node_modules/@angular/core/fesm5/core.js");
/* harmony import */ var _angular_common__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! @angular/common */ "./node_modules/@angular/common/fesm5/common.js");
/* harmony import */ var _tests_index_tests_index_component__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! ./tests-index/tests-index.component */ "./src/app/tests/tests-index/tests-index.component.ts");
/* harmony import */ var _angular_router__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! @angular/router */ "./node_modules/@angular/router/fesm5/router.js");
/* harmony import */ var _test_web_api_test_web_api_component__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ./test-web-api/test-web-api.component */ "./src/app/tests/test-web-api/test-web-api.component.ts");
/* harmony import */ var _angular_forms__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! @angular/forms */ "./node_modules/@angular/forms/fesm5/forms.js");
/* harmony import */ var _tests_routes__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ./tests.routes */ "./src/app/tests/tests.routes.ts");








var TestsModule = /** @class */ (function () {
    function TestsModule() {
    }
    TestsModule = tslib__WEBPACK_IMPORTED_MODULE_0__["__decorate"]([
        Object(_angular_core__WEBPACK_IMPORTED_MODULE_1__["NgModule"])({
            declarations: [
                _tests_index_tests_index_component__WEBPACK_IMPORTED_MODULE_3__["TestsIndexComponent"],
                _test_web_api_test_web_api_component__WEBPACK_IMPORTED_MODULE_5__["TestWebApiComponent"]
            ],
            imports: [
                _angular_common__WEBPACK_IMPORTED_MODULE_2__["CommonModule"],
                _angular_forms__WEBPACK_IMPORTED_MODULE_6__["FormsModule"],
                _angular_router__WEBPACK_IMPORTED_MODULE_4__["RouterModule"].forChild(_tests_routes__WEBPACK_IMPORTED_MODULE_7__["ROUTES"])
            ]
        })
    ], TestsModule);
    return TestsModule;
}());



/***/ }),

/***/ "./src/app/tests/tests.routes.ts":
/*!***************************************!*\
  !*** ./src/app/tests/tests.routes.ts ***!
  \***************************************/
/*! exports provided: ROUTES */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "ROUTES", function() { return ROUTES; });
/* harmony import */ var _tests_index_tests_index_component__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./tests-index/tests-index.component */ "./src/app/tests/tests-index/tests-index.component.ts");
/* harmony import */ var _test_web_api_test_web_api_component__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./test-web-api/test-web-api.component */ "./src/app/tests/test-web-api/test-web-api.component.ts");


var ROUTES = [
    {
        path: "",
        children: [
            { path: "test-web-api", component: _test_web_api_test_web_api_component__WEBPACK_IMPORTED_MODULE_1__["TestWebApiComponent"] },
            { path: "", component: _tests_index_tests_index_component__WEBPACK_IMPORTED_MODULE_0__["TestsIndexComponent"] },
        ]
    }
];


/***/ })

}]);
//# sourceMappingURL=tests-tests-module.js.map