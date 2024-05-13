import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { DxSchedulerModule } from 'devextreme-angular';
import { UserModel } from '../../model/user.model';
import { HttpClient } from '@angular/common/http';
import { FormsModule, NgForm } from '@angular/forms';
import { AppointmentModel } from '../../model/appointment.model';
import { ResultModel } from '../../model/result.model';
import { FormValidateDirective } from 'form-validate-angular';
import { AppointmentDataModel } from '../../model/appointment-data.model';
import { CommonModule } from '@angular/common';
import { AppointmentDeletedEvent } from 'devextreme/ui/scheduler';
import { AuthService } from '../../services/auth.service';

declare const $ : any;


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule,DxSchedulerModule,FormsModule,FormValidateDirective],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{


  appointmentsData: any[] = [];
  selectedDoctorId : string = "";
  currentDate: Date = new Date();
  doctors:UserModel[] = [];

  public addModel: AppointmentModel = new AppointmentModel();
  appointmentData : AppointmentDataModel = new AppointmentDataModel();

  loginUserIsDoctor : boolean = false;

  constructor(
    private http:HttpClient,
    private auth:AuthService){
      if(this.auth.user.userType === "Doctor"){
        this.loginUserIsDoctor = true;
        this.selectedDoctorId = this.auth.user.userId;
        this.getDoctorAppointments();
      }
      else if(this.auth.user.userType === "Patient"){
        
      }
      else{
        this.getAllDoctors();

      }
    }

  ngOnInit(): void {
    this.getAllDoctors();
  }

  getAllDoctors(){
    this.http.get("https://localhost:7203/api/Appointments/GetAllDoctors").subscribe((res:any)=>{
      this.doctors = res.data;
    })
  }

  getDoctorAppointments(){
    if(this.selectedDoctorId == "") return;

    this.http.get(`https://localhost:7203/api/Appointments/GetAllByDoctorId?doctorId=${this.selectedDoctorId}`).subscribe(
      (res:any)=>{

        const data = res.data.map((val:any,i:number)=>{
          return {
            id:val.id,
            text: val.patient.fullName,
            startDate : new Date(val.startDate),
            endDate :  new Date(val.endDate)
          }
        })
        
        this.appointmentsData = data;
      }
    )
  }

  
  onAppointmentFormOpening(event: any) {
    console.log(event);
    this.appointmentData = event.appointmentData;
    const doctorName = this.doctors.find(p=>p.id ==this.selectedDoctorId)?.fullName;
    const specialtyName = this.doctors.find(p=>p.id ==this.selectedDoctorId)?.doctorDetail?.specialtyName;

    this.appointmentData.doctorName = `${doctorName} - ${specialtyName}`
    event.cancel = true;
    $("#addAppointmentModal").modal('show');
  }

  add(form:NgForm){
    if(form.valid){
      const patientId = this.addModel.patient.id === "" ? null : this.addModel.patient.id;
      const data = {
          "doctorId": this.selectedDoctorId,
          "patientId": patientId,
          "firstName": this.addModel.patient.firstName,
          "lastName": this.addModel.patient.lastName,
          "fullAddress": this.addModel.patient.fullAddress,
          "email": this.addModel.patient.email,
          "phoneNumber": this.addModel.patient.phoneNumber,
          "identityNumber": this.addModel.patient.identityNumber,
          "dateOfBirth": this.addModel.patient.dateOfBirth,
          "bloodType": this.addModel.patient.bloodType,
          "startDate": this.appointmentData.startDate,
          "endDate": this.appointmentData.endDate,
          "price": this.doctors.find(p=>p.id == this.addModel.doctorId)?.doctorDetail?.price
      };

      this.http.post("https://localhost:7203/api/Appointments/Create",data).subscribe(res=>{
      $("#addAppointmentModal").modal('hide');
      this.getDoctorAppointments();
      this.addModel = new AppointmentModel();
      });
    }
  }

  findPatientByIdentityNumber(){
    this.http.post<ResultModel<UserModel>>
    (`https://localhost:7203/api/Appointments/FindPatientByIdentityNumber?identityNumber`,
    {identityNumber : this.addModel.patient.identityNumber}).subscribe((res)=>{
      if(res.data !== undefined && res.data !== null){
        this.addModel.patient = res.data;
      }
    })
  }

    
onAppointmentDeleting(event: any) {
    event.cancel = true;
    const result = confirm("You want to delete this appointment?");
    
    if(result){
      const id = event.appointmentData.id;
      console.log(id);
      this.http.get(`https://localhost:7203/api/Appointments/DeleteById?id=${id}`).subscribe(res=>{
          this.getDoctorAppointments();
      })
    }
    
  }
  
}
