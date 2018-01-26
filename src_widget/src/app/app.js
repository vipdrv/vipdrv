(function () {
    var app = angular.module('myApp', ['templates', 'moment-picker', 'ngTextTruncate']);

    // =======================================================================//
    // App Configs                                                            //
    // =======================================================================//

    var apiBaseUrl = '%apiBaseUrl%';
    var defaultSiteId = '%siteId%';

    //=======================================================================//
    // Global Variables                                                      //
    //=======================================================================//

    var widgetTabs = {
        time: {
            id: 'time',
            title: 'Select Date & Time',
            icon: 'svg-white-clock-o.svg',
            isActive: true,
            isLocked: false,
            isCompleted: false
        },
        expert: {
            id: 'expert',
            icon: 'svg-white-users.svg',
            title: 'Select Expert',
            isActive: false,
            isLocked: true,
            isCompleted: false
        },
        beverage: {
            id: 'beverage',
            icon: 'svg-white-coffee.svg',
            title: 'Select Beverage',
            isActive: false,
            isLocked: true,
            isCompleted: false
        },
        road: {
            id: 'road',
            icon: 'svg-white-road.svg',
            title: 'Select Preferred Route',
            isActive: false,
            isLocked: true,
            isCompleted: false
        },
        details: {
            id: 'details',
            icon: 'svg-white-handshake-o.svg',
            title: 'Your Details',
            isActive: false,
            isLocked: true,
            isCompleted: false
        }
    };

    var bookingData = {
        user: {
            firstName: null,
            lastName: null,
            phone: null,
            email: null,
            comment: null
        },
        calendar: {
            date: null,
            time: null,
            dayOfWeek: null,
            isSkipped: null
        },
        expert: {
            id: null,
            img: null,
            name: null,
            description: null,
            isSkipped: null
        },
        beverage: {
            id: null,
            img: null,
            name: null,
            description: null,
            isSkipped: null
        },
        road: {
            id: null,
            img: null,
            name: null,
            description: null,
            isSkipped: null
        },
        vehicle: {
            vin: null,
            stock: null,
            year: null,
            make: null,
            model: null,
            body: null,
            title: null,
            engine: null,
            exterior: null,
            interior: null,
            drivetrain: null,
            transmission: null,
            msrp: null,
            imageUrl: null,
            vdpUrl: null
        }
    };

    var dealerData = {
        siteId: null,
        name: null,
        phone: null,
        address: null,
        siteUrl: null,
        workingHours: [],
        experts: {
            isStepEnabled: true,
            stepOrder: 0,
            items: []
        },
        beverages: {
            isStepEnabled: true,
            stepOrder: 1,
            items: []
        },
        roads: {
            isStepEnabled: true,
            stepOrder: 2,
            items: []
        }
    };

    var globalState = {
        isBookingCompleted: false
    };

    var urlFilters = new FiltersFromUrl(window.location.search).get();
    var siteId = urlFilters.siteId || defaultSiteId;

    app.value('siteId', siteId);
    app.value('widgetTabs', widgetTabs);
    app.value('globalState', globalState);
    app.value('bookingData', bookingData);
    app.value('dealerData', dealerData);
    app.value('apiBaseUrl', apiBaseUrl);
})();