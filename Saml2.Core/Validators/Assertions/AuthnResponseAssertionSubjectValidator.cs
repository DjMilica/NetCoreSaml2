﻿using Saml2.Core.Errors;
using Saml2.Core.Models.Xml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Saml2.Core.Validators.Assertions
{
    public interface IAuthnResponseAssertionSubjectValidator
    {
        Task Validate(Subject subject);
    }

    public class AuthnResponseAssertionSubjectValidator : IAuthnResponseAssertionSubjectValidator
    {
        private readonly INameIdValidator nameIdValidator;
        private readonly IAssertionSubjectConfirmationValidator assertionSubjectConfirmationValidator;

        public AuthnResponseAssertionSubjectValidator(
            INameIdValidator nameIdValidator,
            IAssertionSubjectConfirmationValidator assertionSubjectConfirmationValidator
        ) 
        {
            this.nameIdValidator = nameIdValidator;
            this.assertionSubjectConfirmationValidator = assertionSubjectConfirmationValidator;
        }

        public async Task Validate(Subject subject)
        {
            if (subject == null)
            {
                return;
            }

            if (subject.NameId == null && subject.EncryptedId == null && !(subject.SubjectConfirmations?.Count > 0))
            {
                throw new SamlValidationException("Minimum one of the <SubjectConfirmation> and <NameId> should be defined in <Subject>.");
            }

            this.nameIdValidator.ValidateInSubject(subject);

            await this.assertionSubjectConfirmationValidator.ValidateList(subject.SubjectConfirmations);
        }
    }
}
