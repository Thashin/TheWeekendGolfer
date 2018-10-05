
import { Component, OnInit, Inject, AfterViewInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, FormControl } from "@angular/forms";
import { PartnerService } from "../services/partner.service";
import { first } from "rxjs/operators";
import { Router } from "@angular/router";
import { PlayerService } from '../services/player.service';
import { Player } from '../models/player.model';
import { UserService } from '../services/user.service';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';

@Component({
  templateUrl: './add-partner-dialog.component.html'
})
export class AddPartnerDialogComponent implements OnInit {

  allPlayers: Player[]

  partner= new FormControl('', [Validators.required]);

  constructor(private _playerService: PlayerService, private _partnerService: PartnerService, private _userService: UserService, private formBuilder: FormBuilder,
    public dialog: MatDialogRef<AddPartnerDialogComponent>) {
    
  }

  ngOnInit() {
    this.getPlayers();
  }

  getPlayers() {
    this._userService.getPlayerid().subscribe(player => {
      this._partnerService.getPotentialPartners(player.id).subscribe(potentialPartners => {
        this.allPlayers = potentialPartners;
      });
    });
  }
  onNoClick(): void {
    this.dialog.close();
  }

  submitPartner() {
    this.dialog.close(this.partner.value)
  }

}

