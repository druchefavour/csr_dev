import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Globals } from '../globals';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
    selector: 'app-view',
    templateUrl: 'view.component.html',
    styleUrls: ['view.component.css'],
    providers: [Globals]
})

export class ViewComponent implements OnInit {
    @ViewChild('closeSendActButton') closeSendActButton: any;
    @ViewChild('closeResendActButton') closeResendActButton: any;
    @ViewChild('closeResetButton') closeResetButton: any;
    @ViewChild('closeUnlockButton') closeUnlockButton: any;
    @ViewChild('closeLinkButton') closeLinkButton: any;
    @ViewChild('closeUnlinkButton') closeUnlinkButton: any;
    @ViewChild('closeNullifyButton') closeNullifyButton: any;
    @ViewChild('closeSuspendButton') closeSuspendButton: any;
    @ViewChild('closeUnsuspendButton') closeUnsuspendButton: any;
    @ViewChild('closeClearButton') closeClearButton: any;
    @ViewChild('closeProofingButton') closeProofingButton: any;

    csr: any = '';
    id: any = '';
    login: any = '';
    email: any = '';
    firstName: any = '';
    lastName: any = '';
    ibis_id: any = '';
    driversLicense: any = '';
    streetAddress: any = '';
    city: any = '';
    state: any = '';
    zipCode: any = '';
    primaryPhone: any = '';
    status: any = '';
    birthDate: any = '';
    idproofing_status: any = '';
    idproofing_score: any = '';
    idproofing_LOA: any = '';

    verifyValid: boolean = true;

    passEmail: any = 'active';
    passTemp: any = '';
    tempPassword: any = '';

    passEmailShow: boolean = true;
    passTempShow: boolean = false;

    em: string = '';
    sms: string = '';
    ga: string = '';
    okver: string = '';
    okvert: string = '';
    sq: string = '';
    vc: string = '';
    emailExists: boolean = false;
    smsExists: boolean = false;
    gaExists: boolean = false;
    okverExists: boolean = false;
    okvertExists: boolean = false;
    smsselected: boolean = false;
    emselected: boolean = false;
    gaselected: boolean = false;
    okverselected: boolean = false;
    okvertselected: boolean = false;
    sqExists: boolean = false;
    sqselected: boolean = false;
    vcExists: boolean = false;
    vcselected: boolean = false;

    linkForm = this.fb.group({
        ibis_id: ''
    })
    proofingForm = this.fb.group({
        dateofbirth: '',
        SSN: ['', [Validators.required, Validators.minLength(9)]]
    })
    httpOptions = {
        headers: new HttpHeaders({
            'Access-Control-Allow-Origin': '*',
            'Content-Type': 'application/json'
        })
    };

    constructor(
        private oauthService: OAuthService,
        private fb: FormBuilder,
        private route: Router,
        private http: HttpClient,
        private toastr: ToastrService,
        public globals: Globals
    ) {
        if (sessionStorage.getItem('id') === '')
            this.route.navigate(['/']);
    }

    get givenName() {
        const claims: any = this.oauthService.getIdentityClaims();
        if (!claims) {
            return null;
        }
        this.csr = claims['preferred_username']
        return claims['name'];
    }

    ngOnInit() {
        this.http.post(`${this.globals.url}/api/Factors/listfactors`, { "Id": sessionStorage.getItem('id') }, this.httpOptions)
            .subscribe(
                (data: any) => {
                    if (data.errorCode === undefined) {
                        data.forEach((el: any) => {
                            switch (el.factorType) {
                                case 'email':
                                    this.em = el.id;
                                    this.emailExists = true;
                                    break;
                                case 'sms':
                                    this.sms = el.id;
                                    this.smsExists = true;
                                    break;
                                case 'token:software:totp':
                                    if (el.provider === "GOOGLE") {
                                        this.ga = el.id;
                                        this.gaExists = true;
                                    }
                                    else if (el.provider === "OKTA") {
                                        this.okvert = el.id;
                                        this.okverExists = true;
                                    }
                                    break;
                                case 'push':
                                    this.okver = el.id;
                                    this.okverExists = true;
                                    break;
                                case 'question':
                                    this.sq = el.id;
                                    this.sqExists = true;
                                    break;
                                case 'call':
                                    this.vc = el.id;
                                    this.vcExists = true;
                                    break;
                                default:
                                    break;
                            }
                        });


                    } else this.toastr.error('Please try again later', 'An error has occured!');
                },
                err => this.toastr.error('Please try again later', 'An error has occured!')
            );

        this.csr = sessionStorage.getItem('csr');
        this.id = sessionStorage.getItem('id');
        this.firstName = sessionStorage.getItem('firstName');
        this.lastName = sessionStorage.getItem('lastName');
        this.login = sessionStorage.getItem('username');
        this.email = sessionStorage.getItem('email');
        this.ibis_id = sessionStorage.getItem('ibis_id');
        this.driversLicense = sessionStorage.getItem('driversLicense');
        this.streetAddress = sessionStorage.getItem('streetAddress');
        this.city = sessionStorage.getItem('city');
        this.state = sessionStorage.getItem('state');
        this.zipCode = sessionStorage.getItem('zipCode');
        this.primaryPhone = sessionStorage.getItem('primaryPhone');
        this.status = sessionStorage.getItem('status');
        this.birthDate = sessionStorage.getItem('birthDate');
        this.idproofing_status = sessionStorage.getItem('idproofing_status');
        this.idproofing_score = sessionStorage.getItem('idproofing_score');
        this.idproofing_LOA = sessionStorage.getItem('idproofing_LOA');
    }

    selectEmail() {
        this.passEmail = 'active';
        this.passTemp = '';
        this.passEmailShow = true;
        this.passTempShow = false;
    }

    selectTemp() {
        this.passEmail = '';
        this.passTemp = 'active';
        this.passEmailShow = false;
        this.passTempShow = true;
    }

    SendEmail() {
        this.http.post(`${this.globals.url}/api/userActions/resetpassword`, { "id": this.id }, this.httpOptions)
            .subscribe(
                (data: any) => {
                    if (data.errorCode === undefined) {
                        this.closeResetButton.nativeElement.click();
                        this.toastr.success('Password Email Sent Successfully!')
                    } else this.toastr.error(data.errorSummary, 'An error has occured!');
                },
                err => this.toastr.error('Please try again later', 'An unexpected error has occured!')
            )
    }

    sendActivationEmail() {
        this.http.post(`${this.globals.url}/api/userActions/sendActivationEmail`, { "id": this.id }, this.httpOptions)
            .subscribe(
                (data: any) => {
                    if (data.errorCode === undefined) {
                        this.closeSendActButton.nativeElement.click();
                        this.toastr.success('Activation Email Sent Successfully!')
                    } else this.toastr.error(data.errorSummary, 'An error has occured!');
                },
                err => this.toastr.error('Please try again later', 'An unexpected error has occured!')
            )
    }

    resendActivationEmail() {
        this.http.post(`${this.globals.url}/api/userActions/resendActivationEmail`, { "id": this.id }, this.httpOptions)
            .subscribe(
                (data: any) => {
                    if (data.errorCode === undefined) {
                        this.closeResendActButton.nativeElement.click();
                        this.toastr.success('Reactivation Email Sent Successfully!')
                    } else this.toastr.error(data.errorSummary, 'An error has occured!');
                },
                err => this.toastr.error('Please try again later', 'An unexpected error has occured!')
            )
        }


    setTempPassword() {
        this.http.post(`${this.globals.url}/api/TemporaryPassword/TemporaryPassword`, { "Id": this.id }, this.httpOptions)
            .subscribe(
                (data: any) => {
                    if (data.errorCode === undefined) {
                        this.tempPassword = data.tempPassword;

                        this.toastr.success('Temporary Password Successfully Created!')
                    } else this.toastr.error(data.errorSummary, 'An error has occured!');
                },
                err => this.toastr.error('Please try again later', 'An unexpected error has occured!')
            )
    }

    unlockUser() {
        this.http.post(`${this.globals.url}/api/TemporaryPassword/UnlockUser`, { "Id": this.id }, this.httpOptions)
            .subscribe(
                (data: any) => {
                    if (data.errorCode === undefined) {
                        sessionStorage.setItem('status', data.status);
                        this.status = data.status;

                        this.closeUnlockButton.nativeElement.click();
                        this.toastr.success('User Successfully Unlocked!')
                    } else this.toastr.error(data.errorSummary, 'An error has occured!');
                },
                err => this.toastr.error('Please try again later', 'An unexpected error has occured!')
            )
    }

    linkUserToIbis() {
        this.http.post(`${this.globals.url}/api/LinkUser/linktoibis`, {
            "Id": this.id,
            "Login": this.login,
            "Email": this.email,
            "csrUsername": this.csr,
            "ibis_id": this.linkForm.value.ibis_id
        }, this.httpOptions)
            .subscribe(
                (data: any) => {
                    if (data) {
                        sessionStorage.setItem('ibis_id', this.linkForm.value.ibis_id);
                        this.ibis_id = sessionStorage.getItem('ibis_id');

                        this.closeLinkButton.nativeElement.click();
                        this.toastr.success('User Successfully Linked to IBIS!')
                    } else this.toastr.error('An error has occured while linking to IBIS!');
                },
                err => this.toastr.error('Please try again later', 'An unexpected error has occured!')
            )
    }

    unlinkUserToIbis() {
        this.http.post(`${this.globals.url}/api/UnlinkUser/unlinkfromibis`, {
            "Id": this.id,
            "Login": this.login,
            "Email": this.email,
            "csrUsername": this.csr,
            "ibis_id": this.ibis_id
        }, this.httpOptions)
            .subscribe(
                (data: any) => {
                    if (data) {
                        sessionStorage.setItem('ibis_id', "");
                        this.ibis_id = sessionStorage.getItem('ibis_id');

                        this.closeUnlinkButton.nativeElement.click();
                        this.toastr.success('User Successfully Unlinked From IBIS!')
                    } else this.toastr.error('An error has occured while un-linking to IBIS!');
                },
                err => this.toastr.error('Please try again later', 'An unexpected error has occured!')
            )
    }

    nullifyDL() {
        this.http.post(`${this.globals.url}/api/NullifyCustom/nullifycustom`, {
            "Id": this.id,
            "Login": this.login,
            "Email": this.email,
            "FirstName": this.firstName,
            "LastName": this.lastName,
            "IbisID": this.ibis_id,
            "DriversLicense": this.driversLicense,
            "StreetAddress": this.streetAddress,
            "City": this.city,
            "State": this.state,
            "ZipCode": this.zipCode,
            "PrimaryPhone": this.primaryPhone,
            "BirthDate": this.birthDate
        }, this.httpOptions)
            .subscribe(
                (data: any) => {
                    if (data.errorCode === undefined) {
                        this.closeNullifyButton.nativeElement.click();
                        this.toastr.success('User Drivers License Successfully Nullified!')
                    } else this.toastr.error(data.errorSummary, 'An error has occured!');
                },
                err => this.toastr.error('Please try again later', 'An unexpected error has occured!')
            )
    }

    suspendUser() {
        this.http.post(`${this.globals.url}/api/SuspendUser/suspenduser`, { "Id": this.id }, this.httpOptions)
            .subscribe(
                (data: any) => {
                    if (data.errorCode === undefined) {
                        sessionStorage.setItem('status', 'SUSPENDED');
                        this.status = 'SUSPENDED';

                        this.closeSuspendButton.nativeElement.click();
                        this.toastr.success('User Successfully Suspended!')
                    } else this.toastr.error(data.errorSummary, 'An error has occured!');
                },
                err => this.toastr.error('Please try again later', 'An unexpected error has occured!')
            )
    }

    unsuspendUser() {
        this.http.post(`${this.globals.url}/api/SuspendUser/unsuspenduser`, { "Id": this.id }, this.httpOptions)
            .subscribe(
                (data: any) => {
                    if (data.errorCode === undefined) {
                        sessionStorage.setItem('status', data.status);
                        this.status = data.status;

                        this.closeUnsuspendButton.nativeElement.click();
                        this.toastr.success('User Successfully Un-Suspended!')
                    } else this.toastr.error(data.errorSummary, 'An error has occured!');
                },
                err => this.toastr.error('Please try again later', 'An unexpected error has occured!')
            )
    }

    onSMSChange(evt) { this.smsselected = !this.smsselected }
    onEMChange(evt) { this.emselected = !this.emselected }
    onGAChange(evt) { this.gaselected = !this.gaselected }
    onOKChange(evt) { this.okverselected = !this.okverselected }
    onSQChange(evt) { this.sqselected = !this.sqselected }
    onVCChange(evt) { this.vcselected = !this.vcselected }

    clearFactors() {
        var postObj: any = { 'UserId': this.id };
        if (this.smsselected) postObj.Idsms = this.sms;
        if (this.emselected) postObj.Idem = this.em;
        if (this.gaselected) postObj.IdGA = this.ga;
        if (this.okverselected) {
            postObj.Idokver = this.okver;
            postObj.Idokvert = this.okvert;
        };
        if (this.sqselected) postObj.Idsq = this.sq;
        if (this.vcselected) postObj.Idvc = this.vc;

        this.http.post(`${this.globals.url}/api/Factors/clearfactors`, postObj, this.httpOptions)
            .subscribe(
                (data: any) => {
                    if (data.errorCode === undefined) {
                        if (this.smsselected) this.smsExists = false;
                        if (this.emselected) this.emailExists = false;
                        if (this.gaselected) this.gaExists = false;
                        if (this.okverselected) this.okverExists = false;
                        if (this.sqselected) this.sqExists = false;
                        if (this.vcselected) this.vcExists = false;

                        this.closeClearButton.nativeElement.click();
                        this.toastr.success('Factors Successfully Reset!');
                    } else this.toastr.error(data.errorSummary, 'An error has occured!');
                },
                err => this.toastr.error('Please try again later', 'An unexpected error has occured!')
            )
    }

    onProofingSubmit() {
        // if (this.proofingForm.value.dateofbirth !== '' && this.proofingForm.value.SSN !== '') {
        // this.verifyValid = true;
        // var ssn: string = this.proofingForm.value.SSN;
        // var ssn1 = ssn.slice(0, 3);
        // var ssn2 = ssn.slice(3, 5);
        // var ssn3 = ssn.slice(5, 9);
        // var newSSN = ssn1 + '-' + ssn2 + '-' + ssn3;

        // var obj = {
        //     Id: this.id
        // firstName: this.firstName,
        // lastName: this.lastName,
        // email: this.email,
        // login: this.login,
        // primaryPhone: this.primaryPhone,
        // drivers_license: this.driversLicense,
        // mobilePhone: '',
        // streetAddress: this.streetAddress,
        // ibis_id: this.ibis_id,
        // city: this.city,
        // state: this.state,
        // zipCode: this.zipCode,
        // idproofing_status: 'ibis_override',
        // idproofing_score: '',
        // idproofing_LOA: '4'
        // dateofbirth: this.proofingForm.value.dateofbirth,
        // SSN: newSSN
        // }


        this.http.post(`${this.globals.url}/api/IdProofing/IdProofParameters`, { Id: this.id }, this.httpOptions)
            .subscribe(
                (data: any) => {
                    if (data.errorCode === undefined) {
                        this.closeProofingButton.nativeElement.click();
                        this.toastr.success('User Id-Proofing Details Successfully Updated!')
                    } else this.toastr.error(data.errorSummary, 'An error has occured!');
                },
                err => this.toastr.error('Please try again later', 'An unexpected error has occured!')
            )
        // } else {
        //     this.verifyValid = false;
        // }
    }
}