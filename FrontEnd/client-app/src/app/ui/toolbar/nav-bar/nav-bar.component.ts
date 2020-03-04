import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/shared/services';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {

  menuItems = [{
    text: 'Trang cá nhân',
    icon: 'user'
  }, {
    text: 'Đăng xuất',
    icon: 'runner',
    onClick: () => {
      this.authService.logOut();
    }
  }];

  menuMode = 'context';
  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  changeOptionContext() {
    if (this.menuMode === 'context') {
      this.menuMode = 'list';
    } else {
      this.menuMode = 'context';
    }
  }

}
